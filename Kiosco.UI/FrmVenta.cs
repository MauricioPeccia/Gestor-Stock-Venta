using System.ComponentModel;
using Kiosco.Entidades;
using Kiosco.Servicios;

namespace Kiosco.UI;

public partial class FrmVenta : Form
{
    private readonly BindingList<VentaItem> _items = new();
    private readonly ProductoServicio _productos = new();
    private readonly VentaServicio _ventas = new();

    public FrmVenta()
    {
        InitializeComponent();
    }

    private void FrmVenta_Load(object sender, EventArgs e)
    {
        dgvItems.AutoGenerateColumns = false;
        dgvItems.DataSource = _items;
        ActualizarTotal();
        txtCodigo.Focus();
    }

    private async void txtCodigo_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter) return;
        e.SuppressKeyPress = true; // Evita el "ding" de Windows

        var texto = txtCodigo.Text.Trim();
        txtCodigo.Clear();
        if (string.IsNullOrWhiteSpace(texto)) return;

        decimal multiplicador = 1;
        var partes = texto.Split('*');
        if (partes.Length == 2 && decimal.TryParse(partes[0].Trim(), out var cantPrefijo) && cantPrefijo > 0)
        {
            multiplicador = cantPrefijo;
            texto = partes[1].Trim();
        }

        try
        {
            var prod = await _productos.ObtenerPorCodigoAsync(texto);
            if (prod == null)
            {
                MessageBox.Show("Producto no encontrado con el código ingresado.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCodigo.Focus();
                return;
            }

            decimal cantidad = multiplicador;
            if (prod.Unidad != "UN")
            {
                // Para productos fraccionables (KG / LT): pedir peso si no fue especificado con '*'
                if (partes.Length != 2)
                {
                    cantidad = PedirCantidadPesable(prod);
                    if (cantidad <= 0)
                    {
                        txtCodigo.Focus();
                        return;
                    }
                }
            }
            else
            {
                // En unidades 'UN', se venden en enteros
                cantidad = Math.Floor(cantidad);
                if (cantidad <= 0) cantidad = 1;
            }

            var existente = _items.FirstOrDefault(i => i.ProductoId == prod.Id);
            if (existente != null && prod.Unidad == "UN")
            {
                existente.Cantidad += cantidad;
            }
            else
            {
                _items.Add(new VentaItem
                {
                    ProductoId = prod.Id,
                    Nombre = prod.Nombre,
                    Unidad = prod.Unidad,
                    Cantidad = cantidad,
                    PrecioUnitario = prod.PrecioVenta
                });
            }

            _items.ResetBindings(); // Subtotal es calculado: refrescar la grilla
            ActualizarTotal();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al consultar el producto: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            txtCodigo.Focus();
        }
    }

    private decimal PedirCantidadPesable(Producto prod)
    {
        using var form = new Form
        {
            Width = 380,
            Height = 220,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            Text = $"Ingreso de cantidad - {prod.Nombre}",
            StartPosition = FormStartPosition.CenterParent,
            MaximizeBox = false,
            MinimizeBox = false
        };

        var lbl = new Label
        {
            Left = 20,
            Top = 15,
            Width = 330,
            Height = 45,
            Text = $"Ingrese el peso en gramos (ej. 250 = 0,250 {prod.Unidad}) o decimal directo con coma:"
        };

        var txt = new TextBox
        {
            Left = 20,
            Top = 65,
            Width = 320,
            Font = new Font("Segoe UI", 12F)
        };

        var btnOk = new Button
        {
            Text = "Aceptar",
            Left = 130,
            Top = 120,
            Width = 100,
            Height = 38,
            DialogResult = DialogResult.OK
        };

        var btnCancel = new Button
        {
            Text = "Cancelar",
            Left = 240,
            Top = 120,
            Width = 100,
            Height = 38,
            DialogResult = DialogResult.Cancel
        };

        form.Controls.AddRange(new Control[] { lbl, txt, btnOk, btnCancel });
        form.AcceptButton = btnOk;
        form.CancelButton = btnCancel;

        if (form.ShowDialog(this) == DialogResult.OK)
        {
            var valStr = txt.Text.Trim().Replace('.', ',');
            if (decimal.TryParse(valStr, out var valor) && valor > 0)
            {
                // Si tipean > 10 en productos de KG, interpretamos como gramos (ej: 250 -> 0.250)
                if (prod.Unidad == "KG" && valor >= 10)
                {
                    return Math.Round(valor / 1000m, 3);
                }
                return Math.Round(valor, 3);
            }
        }

        return 0;
    }

    private void ActualizarTotal()
    {
        var total = _items.Sum(i => i.Subtotal);
        lblTotal.Text = total.ToString("C2");
        btnCobrar.Enabled = _items.Count > 0;
    }

    private async void btnCobrar_Click(object sender, EventArgs e)
    {
        await ProcesarCobroAsync();
    }

    private async Task ProcesarCobroAsync()
    {
        if (_items.Count == 0)
        {
            MessageBox.Show("No hay productos en el carrito para cobrar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtCodigo.Focus();
            return;
        }

        var total = _items.Sum(i => i.Subtotal);

        using var frmCobro = new FrmCobro(total);
        if (frmCobro.ShowDialog(this) != DialogResult.OK)
        {
            txtCodigo.Focus();
            return;
        }

        var venta = new Venta
        {
            Items = _items.ToList()
        };

        if (frmCobro.AplicadoEfectivo > 0)
        {
            venta.Pagos.Add(new PagoVenta
            {
                Metodo = "EFECTIVO",
                Monto = frmCobro.AplicadoEfectivo
            });
        }

        if (frmCobro.Transferencia > 0)
        {
            venta.Pagos.Add(new PagoVenta
            {
                Metodo = "TRANSFERENCIA",
                Monto = frmCobro.Transferencia
            });
        }

        try
        {
            btnCobrar.Enabled = false;
            long ventaId;

            try
            {
                ventaId = await _ventas.RegistrarAsync(venta, permitirSinStock: false);
            }
            catch (StockInsuficienteException ex)
            {
                var respuesta = MessageBox.Show(
                    $"No hay suficiente stock registrado de '{ex.Producto}' (disponible: {ex.Disponible}).\n\n¿Desea vender igual dejando el stock en negativo?",
                    "Stock Insuficiente",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (respuesta == DialogResult.Yes)
                {
                    ventaId = await _ventas.RegistrarAsync(venta, permitirSinStock: true);
                }
                else
                {
                    return;
                }
            }

            MessageBox.Show(
                $"¡Venta #{ventaId} registrada con éxito!\n\n" +
                $"Total cobrado: {total:C2}\n" +
                $"Vuelto a entregar: {frmCobro.Vuelto:C2}",
                "Venta Completada",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            LimpiarVenta();
        }
        catch (NegocioException ex)
        {
            // Fase 7.3: Si lanza NegocioException, mostrar el mensaje y NO limpiar la pantalla
            MessageBox.Show(ex.Message, "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ocurrió un error al registrar la venta: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnCobrar.Enabled = _items.Count > 0;
            txtCodigo.Focus();
        }
    }

    private void btnQuitarItem_Click(object sender, EventArgs e)
    {
        QuitarItemSeleccionado();
    }

    private void QuitarItemSeleccionado()
    {
        if (dgvItems.CurrentRow?.DataBoundItem is VentaItem item)
        {
            _items.Remove(item);
            _items.ResetBindings();
            ActualizarTotal();
        }
        txtCodigo.Focus();
    }

    private void btnCancelarVenta_Click(object sender, EventArgs e)
    {
        if (_items.Count == 0) return;

        var confirmar = MessageBox.Show("¿Está seguro de cancelar la venta actual y vaciar el carrito?", "Cancelar Venta", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (confirmar == DialogResult.Yes)
        {
            LimpiarVenta();
        }
    }

    private void LimpiarVenta()
    {
        _items.Clear();
        _items.ResetBindings();
        ActualizarTotal();
        txtCodigo.Clear();
        txtCodigo.Focus();
    }

    private async void btnBuscar_Click(object sender, EventArgs e)
    {
        await BuscarProductoAsync();
    }

    private async Task BuscarProductoAsync()
    {
        using var form = new Form
        {
            Width = 600,
            Height = 450,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            Text = "Buscar Producto (por nombre o código)",
            StartPosition = FormStartPosition.CenterParent,
            MaximizeBox = false,
            MinimizeBox = false
        };

        var txtBusqueda = new TextBox
        {
            Left = 20,
            Top = 15,
            Width = 440,
            Font = new Font("Segoe UI", 11F),
            PlaceholderText = "Escriba parte del nombre o código..."
        };

        var btnBuscarDlg = new Button
        {
            Text = "Buscar",
            Left = 470,
            Top = 15,
            Width = 90,
            Height = 35
        };

        var lstResultados = new ListBox
        {
            Left = 20,
            Top = 60,
            Width = 540,
            Height = 300,
            Font = new Font("Segoe UI", 10F)
        };

        var btnSeleccionar = new Button
        {
            Text = "Seleccionar",
            Left = 330,
            Top = 365,
            Width = 110,
            Height = 35,
            DialogResult = DialogResult.OK
        };

        var btnCancelarDlg = new Button
        {
            Text = "Cancelar",
            Left = 450,
            Top = 365,
            Width = 110,
            Height = 35,
            DialogResult = DialogResult.Cancel
        };

        List<Producto> lista = new();

        async Task EjecutarBusqueda()
        {
            var texto = txtBusqueda.Text.Trim();
            if (string.IsNullOrWhiteSpace(texto)) return;
            btnBuscarDlg.Enabled = false;
            try
            {
                lista = await _productos.BuscarAsync(texto);
                lstResultados.Items.Clear();
                foreach (var p in lista)
                {
                    var cod = string.IsNullOrWhiteSpace(p.Codigo) ? "S/C" : p.Codigo;
                    lstResultados.Items.Add($"[{cod}] {p.Nombre} - {p.PrecioVenta:C2} ({p.Unidad}) | Stock: {p.StockActual}");
                }
            }
            finally
            {
                btnBuscarDlg.Enabled = true;
            }
        }

        btnBuscarDlg.Click += async (s, ev) => await EjecutarBusqueda();
        txtBusqueda.KeyDown += async (s, ev) =>
        {
            if (ev.KeyCode == Keys.Enter)
            {
                ev.SuppressKeyPress = true;
                await EjecutarBusqueda();
            }
        };

        lstResultados.DoubleClick += (s, ev) =>
        {
            if (lstResultados.SelectedIndex >= 0)
            {
                form.DialogResult = DialogResult.OK;
                form.Close();
            }
        };

        form.Controls.AddRange(new Control[] { txtBusqueda, btnBuscarDlg, lstResultados, btnSeleccionar, btnCancelarDlg });

        if (form.ShowDialog(this) == DialogResult.OK && lstResultados.SelectedIndex >= 0 && lstResultados.SelectedIndex < lista.Count)
        {
            var p = lista[lstResultados.SelectedIndex];
            txtCodigo.Text = p.Codigo ?? p.Nombre;
            txtCodigo_KeyDown(txtCodigo, new KeyEventArgs(Keys.Enter));
        }

        txtCodigo.Focus();
    }

    private void FrmVenta_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F5)
        {
            e.SuppressKeyPress = true;
            btnCobrar.PerformClick();
        }
        else if (e.KeyCode == Keys.F2)
        {
            e.SuppressKeyPress = true;
            btnBuscar.PerformClick();
        }
        else if (e.KeyCode == Keys.Delete)
        {
            if (!txtCodigo.Focused)
            {
                e.SuppressKeyPress = true;
                btnQuitarItem.PerformClick();
            }
        }
        else if (e.KeyCode == Keys.Escape)
        {
            e.SuppressKeyPress = true;
            btnCancelarVenta.PerformClick();
        }
    }
}

