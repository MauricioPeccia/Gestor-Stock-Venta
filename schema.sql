-- ========================================================
-- Script de Base de Datos para Gestor-Stock-Venta (MySQL)
-- Compatible con Servicios, Entidades y UI del proyecto
-- ========================================================

CREATE DATABASE IF NOT EXISTS `gestorkiosco`
  DEFAULT CHARACTER SET utf8mb4
  COLLATE utf8mb4_unicode_ci;

USE `gestorkiosco`;

-- 1. Tabla de Categorías
CREATE TABLE IF NOT EXISTS `categorias` (
    `id` INT AUTO_INCREMENT PRIMARY KEY,
    `nombre` VARCHAR(100) NOT NULL,
    `activo` TINYINT(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB;

-- 2. Tabla de Proveedores
CREATE TABLE IF NOT EXISTS `proveedores` (
    `id` INT AUTO_INCREMENT PRIMARY KEY,
    `nombre` VARCHAR(150) NOT NULL,
    `telefono` VARCHAR(50) NULL,
    `email` VARCHAR(100) NULL,
    `direccion` VARCHAR(255) NULL,
    `activo` TINYINT(1) NOT NULL DEFAULT 1
) ENGINE=InnoDB;

-- 3. Tabla de Productos
CREATE TABLE IF NOT EXISTS `productos` (
    `id` INT AUTO_INCREMENT PRIMARY KEY,
    `codigo` VARCHAR(100) UNIQUE NULL,
    `nombre` VARCHAR(255) NOT NULL,
    `categoria_id` INT NULL,
    `unidad` VARCHAR(10) NOT NULL DEFAULT 'UN', -- 'UN', 'KG', 'LT'
    `precio_costo` DECIMAL(12, 2) NOT NULL DEFAULT 0.00,
    `precio_venta` DECIMAL(12, 2) NOT NULL DEFAULT 0.00,
    `stock_actual` DECIMAL(12, 3) NOT NULL DEFAULT 0.000,
    `stock_minimo` DECIMAL(12, 3) NOT NULL DEFAULT 0.000,
    `activo` TINYINT(1) NOT NULL DEFAULT 1,
    INDEX `idx_productos_codigo` (`codigo`),
    INDEX `idx_productos_nombre` (`nombre`),
    INDEX `idx_productos_activo` (`activo`),
    CONSTRAINT `fk_productos_categoria` FOREIGN KEY (`categoria_id`) REFERENCES `categorias` (`id`) ON DELETE SET NULL
) ENGINE=InnoDB;

-- 4. Historial de Precios
CREATE TABLE IF NOT EXISTS `historial_precios` (
    `id` BIGINT AUTO_INCREMENT PRIMARY KEY,
    `producto_id` INT NOT NULL,
    `costo_anterior` DECIMAL(12, 2) NOT NULL,
    `costo_nuevo` DECIMAL(12, 2) NOT NULL,
    `venta_anterior` DECIMAL(12, 2) NOT NULL,
    `venta_nuevo` DECIMAL(12, 2) NOT NULL,
    `fecha` DATETIME NOT NULL,
    INDEX `idx_historial_producto` (`producto_id`),
    CONSTRAINT `fk_historial_producto` FOREIGN KEY (`producto_id`) REFERENCES `productos` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- 5. Jornadas de Caja
CREATE TABLE IF NOT EXISTS `jornadas_caja` (
    `id` BIGINT AUTO_INCREMENT PRIMARY KEY,
    `fecha` DATE NOT NULL,
    `apertura_en` DATETIME NOT NULL,
    `monto_apertura` DECIMAL(12, 2) NOT NULL DEFAULT 0.00,
    `cierre_en` DATETIME NULL,
    `efectivo_esperado` DECIMAL(12, 2) NULL,
    `efectivo_contado` DECIMAL(12, 2) NULL,
    `diferencia` DECIMAL(12, 2) NULL,
    `estado` VARCHAR(20) NOT NULL DEFAULT 'ABIERTA', -- 'ABIERTA', 'CERRADA'
    `observaciones` TEXT NULL,
    INDEX `idx_jornada_estado` (`estado`),
    INDEX `idx_jornada_fecha` (`fecha`)
) ENGINE=InnoDB;

-- 6. Compras
CREATE TABLE IF NOT EXISTS `compras` (
    `id` BIGINT AUTO_INCREMENT PRIMARY KEY,
    `proveedor_id` INT NULL,
    `nro_comprobante` VARCHAR(50) NULL,
    `fecha` DATETIME NOT NULL,
    `total` DECIMAL(12, 2) NOT NULL DEFAULT 0.00,
    `pagada_con_caja` TINYINT(1) NOT NULL DEFAULT 0,
    `observaciones` TEXT NULL,
    CONSTRAINT `fk_compras_proveedor` FOREIGN KEY (`proveedor_id`) REFERENCES `proveedores` (`id`) ON DELETE SET NULL
) ENGINE=InnoDB;

-- 7. Detalle de Compras
CREATE TABLE IF NOT EXISTS `compra_detalle` (
    `id` BIGINT AUTO_INCREMENT PRIMARY KEY,
    `compra_id` BIGINT NOT NULL,
    `producto_id` INT NOT NULL,
    `cantidad` DECIMAL(12, 3) NOT NULL,
    `costo_unitario` DECIMAL(12, 2) NOT NULL,
    `subtotal` DECIMAL(12, 2) NOT NULL,
    CONSTRAINT `fk_detalle_compra` FOREIGN KEY (`compra_id`) REFERENCES `compras` (`id`) ON DELETE CASCADE,
    CONSTRAINT `fk_detalle_compra_producto` FOREIGN KEY (`producto_id`) REFERENCES `productos` (`id`)
) ENGINE=InnoDB;

-- 8. Movimientos de Caja
CREATE TABLE IF NOT EXISTS `movimientos_caja` (
    `id` BIGINT AUTO_INCREMENT PRIMARY KEY,
    `jornada_id` BIGINT NOT NULL,
    `tipo` VARCHAR(20) NOT NULL, -- 'INGRESO', 'EGRESO'
    `concepto` VARCHAR(255) NOT NULL,
    `monto` DECIMAL(12, 2) NOT NULL,
    `origen` VARCHAR(30) NULL,   -- 'COMPRA', 'MANUAL', etc.
    `compra_id` BIGINT NULL,
    `fecha` DATETIME NOT NULL,
    CONSTRAINT `fk_mov_caja_jornada` FOREIGN KEY (`jornada_id`) REFERENCES `jornadas_caja` (`id`) ON DELETE CASCADE,
    CONSTRAINT `fk_mov_caja_compra` FOREIGN KEY (`compra_id`) REFERENCES `compras` (`id`) ON DELETE SET NULL
) ENGINE=InnoDB;

-- 9. Ventas
CREATE TABLE IF NOT EXISTS `ventas` (
    `id` BIGINT AUTO_INCREMENT PRIMARY KEY,
    `jornada_id` BIGINT NOT NULL,
    `fecha` DATETIME NOT NULL,
    `subtotal` DECIMAL(12, 2) NOT NULL DEFAULT 0.00,
    `descuento` DECIMAL(12, 2) NOT NULL DEFAULT 0.00,
    `total` DECIMAL(12, 2) NOT NULL DEFAULT 0.00,
    `estado` VARCHAR(20) NOT NULL DEFAULT 'COMPLETADA', -- 'COMPLETADA', 'ANULADA'
    `fecha_anulacion` DATETIME NULL,
    `motivo_anulacion` VARCHAR(255) NULL,
    INDEX `idx_ventas_jornada` (`jornada_id`),
    CONSTRAINT `fk_ventas_jornada` FOREIGN KEY (`jornada_id`) REFERENCES `jornadas_caja` (`id`)
) ENGINE=InnoDB;

-- 10. Detalle de Ventas
CREATE TABLE IF NOT EXISTS `venta_detalle` (
    `id` BIGINT AUTO_INCREMENT PRIMARY KEY,
    `venta_id` BIGINT NOT NULL,
    `producto_id` INT NOT NULL,
    `cantidad` DECIMAL(12, 3) NOT NULL,
    `precio_unitario` DECIMAL(12, 2) NOT NULL,
    `costo_unitario` DECIMAL(12, 2) NOT NULL,
    `subtotal` DECIMAL(12, 2) NOT NULL,
    CONSTRAINT `fk_detalle_venta` FOREIGN KEY (`venta_id`) REFERENCES `ventas` (`id`) ON DELETE CASCADE,
    CONSTRAINT `fk_detalle_venta_producto` FOREIGN KEY (`producto_id`) REFERENCES `productos` (`id`)
) ENGINE=InnoDB;

-- 11. Formas de Pago de Ventas
CREATE TABLE IF NOT EXISTS `venta_pagos` (
    `id` BIGINT AUTO_INCREMENT PRIMARY KEY,
    `venta_id` BIGINT NOT NULL,
    `metodo` VARCHAR(30) NOT NULL, -- 'EFECTIVO', 'TRANSFERENCIA'
    `monto` DECIMAL(12, 2) NOT NULL,
    CONSTRAINT `fk_pagos_venta` FOREIGN KEY (`venta_id`) REFERENCES `ventas` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- 12. Movimientos de Stock
CREATE TABLE IF NOT EXISTS `movimientos_stock` (
    `id` BIGINT AUTO_INCREMENT PRIMARY KEY,
    `producto_id` INT NOT NULL,
    `tipo` VARCHAR(30) NOT NULL, -- 'AJUSTE_POS', 'AJUSTE_NEG', 'VENTA', 'ANULACION_VENTA', 'COMPRA'
    `cantidad` DECIMAL(12, 3) NOT NULL,
    `stock_anterior` DECIMAL(12, 3) NOT NULL,
    `stock_nuevo` DECIMAL(12, 3) NOT NULL,
    `referencia_tipo` VARCHAR(30) NULL, -- 'VENTA', 'COMPRA', etc.
    `referencia_id` BIGINT NULL,
    `motivo` VARCHAR(255) NULL,
    `fecha` DATETIME NOT NULL,
    INDEX `idx_mov_stock_producto` (`producto_id`),
    CONSTRAINT `fk_mov_stock_producto` FOREIGN KEY (`producto_id`) REFERENCES `productos` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB;

-- Opcional: Crear usuario con permisos si se utiliza en local
-- CREATE USER IF NOT EXISTS 'app_kiosco'@'localhost' IDENTIFIED BY 'main123';
-- GRANT ALL PRIVILEGES ON `gestorkiosco`.* TO 'app_kiosco'@'localhost';
-- FLUSH PRIVILEGES;

