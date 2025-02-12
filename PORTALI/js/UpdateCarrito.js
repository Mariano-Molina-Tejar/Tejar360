function updateListaPrice(ListaPrecios) {
    const cookieData = getCookie('Cotizacion_A1');
    if (cookieData) {
        const parsedData = JSON.parse(cookieData);
        console.log(parsedData);

        // Aquí se accede directamente al encabezado y se actualiza el PriceId
        if (parsedData.PriceId !== undefined) {
            parsedData.PriceId = ListaPrecios; // Asumiendo que ListaPrecios es el nuevo valor para PriceId
        }
        // Guardar la cookie actualizada
        document.cookie = `Cotizacion_A1=${JSON.stringify(parsedData)};path=/`;
    }
}
/*ACTUALIZA LA LISTA DE PRECIOS*/
function updateListaProductos(ListaPrecios) {
    const rows = document.querySelectorAll("table.shoping-cart-table tbody tr");    
    rows.forEach(row => {
        // Obtener elementos de la fila
        const lineId = row.querySelector("#LineId").textContent.trim();
        const itemCodeElement = row.querySelector("#ItemCode");
        const priceElement = row.querySelector("#Price");
        const quantityInput = row.querySelector("#counterInput");
        const lineTotalElement = row.querySelector("#LineTotal");

        // Convertir valores a números
        const itemCode = itemCodeElement?.textContent.trim() || "";
        const price = parseFloat(priceElement?.textContent.trim().replace("Q.", "")) || 0;
        const quantity = parseFloat(quantityInput?.value) || 0;

        // Calcular el total de la línea
        const lineTotal = price * quantity;        

        // Actualizar el total en el DOM
        if (lineTotalElement) {
            lineTotalElement.innerHTML = `<h4 style="font-size:18px"><strong>Q.${lineTotal.toFixed(2)}</strong></h4>`;
        }

        // Actualizar la cookie
        updateCookieItem(itemCode, lineId, price, quantity);        
    });
}


function updateSoloQuantity(pList) {
    const listaPrecios = pList;
    let cookieName = obtenerNombreCookieMasReciente("T-OneCotizacion-");
    const cookieData = getCookie(cookieName);

    if (cookieData) {
        const parsedData = JSON.parse(cookieData);
        parsedData.PriceId = listaPrecios;

        const rows = document.querySelectorAll("table.shoping-cart-table tbody tr");

        rows.forEach(row => {
            const lineId = row.querySelector("#LineId")?.textContent.trim() || "";
            const itemCodeElement = row.querySelector("#ItemCode");
            const priceUnitElement = row.querySelector("#productUnitPrice strong");
            const quantityInput = row.querySelector("#counterInput");
            const DescuentoElement = row.querySelector("#iDscto strong");

            const itemCode = itemCodeElement?.textContent.trim() || "";
            const priceText = priceUnitElement?.textContent.trim() || "0";
            const DescuentoText = DescuentoElement?.textContent.trim() || "0";

            const quantity = parseFloat(quantityInput?.value) || 0;
            const priceUnit = parseFloat(priceText.replace(/[^0-9.]/g, '')) || 0;
            const descuento = parseFloat(DescuentoText.replace(/[^0-9.]/g, '')) || 0;

            const item = parsedData.productos.find(product =>
                product.ItemCode.toString().trim() === itemCode.toString().trim() &&
                product.LineId.toString().trim() === lineId.toString().trim()
            );

            if (item) {
                item.ListPrice = listaPrecios;
                item.Quantity = quantity;
                item.Dscto = descuento;
                item.LineTotal = ((priceUnit - descuento) * quantity);
            }
        });

        // Actualiza la cookie una vez después del bucle
        const expires = new Date();
        expires.setTime(expires.getTime() + 7 * 24 * 60 * 60 * 1000); // 7 días
        document.cookie = cookieName + "=" + JSON.stringify(parsedData) + "; path=/; expires=" + expires.toUTCString();        
    } else {
        console.warn("No se encontró la cookie " + cookieName);
    }
}


/*UPDATE SOLO CANTIDADES*/
function ModificarCantidades(llave, cantidad, Descuento) {
    let cookieName = obtenerNombreCookieMasReciente("T-OneCotizacion-");
    const cookieData = getCookie(cookieName);

    if (cookieData) {
        const parsedData = JSON.parse(cookieData);

        if (parsedData.productos && Array.isArray(parsedData.productos)) {
            const producto = parsedData.productos.find(p => p.Identity.trim() === llave.trim());
            if (producto) {
                producto.Quantity = cantidad;
                producto.CantidadMetros = cantidad;
                producto.DescuentoU = Descuento;
            } else {
                alert("No se encontró el producto con la llave: " + llave.trim());
                return;
            }
        } else {
            alert("No se encontraron productos en la cookie.");
            return;
        }

        // Eliminar la cookie antes de actualizarla
        document.cookie = cookieName + "=; path=/; expires=Thu, 01 Jan 1970 00:00:00 UTC;";

        // Crear la nueva cookie con la información actualizada
        const expires = new Date();
        expires.setTime(expires.getTime() + 7 * 24 * 60 * 60 * 1000); // 7 días
        const nuevaCookie = cookieName + "=" + JSON.stringify(parsedData) + "; path=/; expires=" + expires.toUTCString() + ";";
        document.cookie = nuevaCookie;
    } else {
        console.warn("No se encontró la cookie " + cookieName);
    }
}


/*CREA EL COOKIE DE COTIZACION*/
function CrearCotizacionCookie(clienteId, producto) {
    let cookieName = obtenerNombreCookieMasReciente("T-OneCotizacion-");
    var llave = generarNumeroCotizacion();
    // Si no existe una cotización previa, crea una nueva
    if (!cookieName) {

        cookieName = "T-OneCotizacion-" + llave;
    }

    const existingCotizacion = getCookie(cookieName);
    let cotizacion;
    if (existingCotizacion) {
        cotizacion = JSON.parse(existingCotizacion); // Convertimos el JSON a un objeto
        cotizacion.productos.push(producto); // Agregamos el nuevo producto
    } else {
        cotizacion = {
            Llave: llave,
            NoCotizacion: generarNumeroCotizacion(),
            cliente: clienteId,
            LicTradNum: "",
            CardCode: "",
            CardName: "",
            Direccion: "",
            Email: "",
            Telefono: "",
            Fecha: new Date().toISOString().split('T')[0], // Fecha actual
            PriceId: 9,
            FacturarNit: "",
            FacturarNombre: "",
            FacturarDireccion: "",
            EsCF: "",
            Borrador: "N",
            productos: [producto]
        };
    }

    // Guardar la cotización actualizada
    setCookie(cookieName, JSON.stringify(cotizacion), 7);

    // Actualizar el badge del carrito
    actualizarCarritoBadge(cotizacion.productos.length);

    return true;
}

function setCookie(name, value, days) {
    const date = new Date();
    date.setTime(date.getTime() + days * 24 * 60 * 60 * 1000);
    document.cookie = `${name}=${value};expires=${date.toUTCString()};path=/`;
}

/*UPDATE SOCIO DE NEGOCIO*/
function updateSocioNegocio(socioNegocio) {
    let cookieName = obtenerNombreCookieMasReciente("T-OneCotizacion-");

    if (!cookieName) {
        console.warn("No se encontró ninguna cookie de cotización.");
        return;
    }

    const cookieData = getCookie(cookieName);
    if (cookieData) {
        const parsedData = JSON.parse(decodeURIComponent(cookieData)); // ← Aquí decodificamos la cookie

        parsedData.Address = socioNegocio.Address;
        parsedData.CardCode = socioNegocio.CardCode;
        parsedData.CardName = socioNegocio.CardName;
        parsedData.Email = socioNegocio.Email;
        parsedData.Phone = socioNegocio.Phone;
        parsedData.LicTradNum = socioNegocio.LicTradNum;
        parsedData.FacturarNit = socioNegocio.FacturarNit;
        parsedData.FacturarNombre = socioNegocio.FacturarNombre;
        parsedData.FacturarDireccion = socioNegocio.FacturarDireccion;
        parsedData.EsCF = socioNegocio.EsCF;

        // Configurar la expiración de la cookie (7 días)
        const expires = new Date();
        expires.setTime(expires.getTime() + 7 * 24 * 60 * 60 * 1000);

        // Guardar la cookie corregida (sin encodeURIComponent)
        document.cookie = `${cookieName}=${JSON.stringify(parsedData)};path=/;expires=${expires.toUTCString()}`;
    } else {
        console.warn(`No se encontró la cookie: ${cookieName}`);
    }
}

function updateDescuentos(Identity, Descuentos) {
    let cookieName = obtenerNombreCookieMasReciente("T-OneCotizacion-");

    if (!cookieName) {
        console.warn("No se encontró ninguna cookie de cotización.");
        return;
    }

    const cookieData = getCookie(cookieName);
    if (cookieData) {
        const parsedData = JSON.parse(decodeURIComponent(cookieData)); // ← Aquí decodificamos la cookie

        if (!parsedData.productos || !Array.isArray(parsedData.productos)) {
            console.warn("La estructura de productos en la cotización es inválida.");
            return;
        }

        // Buscar el producto con el Identity proporcionado
        const producto = parsedData.productos.find(p => p.Identity === Identity);
        if (!producto) {
            console.warn(`No se encontró un producto con Identity: ${Identity}`);
            return;
        }

        // Actualizar solo los valores de descuento dentro del producto encontrado
        producto.DescuentoLpr = parseFloat(Descuentos.DescuentoLpr) || 0.00;
        producto.DescuentoNwp = parseFloat(Descuentos.DescuentoNwp) || 0.00;
        producto.DescuentoPor = parseFloat(Descuentos.DescuentoPor) || 0.00;
        producto.DescuentoQtz = parseFloat(Descuentos.DescuentoQtz) || 0.00;

        // Configurar la expiración de la cookie (7 días)
        const expires = new Date();
        expires.setTime(expires.getTime() + 7 * 24 * 60 * 60 * 1000);

        // Guardar la cookie corregida
        document.cookie = `${cookieName}=${JSON.stringify(parsedData)};path=/;expires=${expires.toUTCString()}`;
    } else {
        console.warn(`No se encontró la cookie: ${cookieName}`);
    }
}

/*OBTIENE EL SOCIO DE NEGOCIO*/
function getSocioNegocio() {
    let cookieName = obtenerNombreCookieMasReciente("T-OneCotizacion-");
    const cookieData = getCookie(cookieName); // Obtiene la cookie.
    if (cookieData) {
        const parsedData = JSON.parse(cookieData); // Convierte la cookie en objeto.

        // Crea un nuevo objeto con solo las propiedades que necesitas.
        const filteredData = {
            Address: parsedData?.Address ?? "",  // Usa optional chaining y coalescencia nula
            CardCode: parsedData?.CardCode ?? "",
            CardName: parsedData?.CardName ?? "",
            Direccion: parsedData?.Direccion ?? "",
            Email: parsedData?.Email ?? "",
            Fecha: parsedData?.Fecha ?? "",
            LicTradNum: parsedData?.LicTradNum ?? "",
            NoCotizacion: parsedData?.NoCotizacion ?? "",
            Phone: parsedData?.Phone ?? "",
            PriceId: parsedData?.PriceId ?? "",
            Telefono: parsedData?.Telefono ?? "",
            cliente: parsedData?.cliente ?? "",
            FacturarNit: parsedData?.FacturarNit ?? "",
            FacturarNombre: parsedData?.FacturarNombre ?? "",
            FacturarDireccion: parsedData?.FacturarDireccion ?? "",
            EsCF: parsedData?.EsCF ?? ""
        };

        return filteredData; // Retorna el objeto filtrado.
    }
    return null; // Retorna null si la cookie no existe.
}

/*ELIMINA LA FILA DEL LISTADO DE PRODUCTOS*/
function eliminarFilaCotizacion(Identity) {
    let cookieName = obtenerNombreCookieMasReciente("T-OneCotizacion-");
    try {
        const cookieData = getCookie(cookieName);
        if (!cookieData) {
            alert("No se encontró la cookie o está vacía.");
            return;
        }

        const parsedData = JSON.parse(cookieData);
        // Asegúrate de que la estructura esperada esté presente
        if (!parsedData.productos || !Array.isArray(parsedData.productos)) {
            alert("Los datos de la cookie no tienen el formato esperado.");
            return;
        }

        // Filtra los productos eliminando el que coincida con itemCode o lineId
        const productosActualizados = parsedData.productos.filter(producto => {
            return producto.Identity !== Identity;
        });

        // Verifica si algún producto fue eliminado
        if (productosActualizados.length === parsedData.productos.length) {
            alert("No se encontró un producto con los datos proporcionados.");
            return;
        }

        // Actualiza el objeto con los productos filtrados
        parsedData.productos = productosActualizados;

        // Guarda nuevamente la cookie con los datos actualizados
        setCookie(cookieName, JSON.stringify(parsedData), 7);
    } catch (error) {        
        alert("Ocurrió un error al intentar eliminar el producto.");
    }
}

/*BORRA LA COOKIE*/
function borrarCookieCotizacion() {
    let cookieName = obtenerNombreCookieMasReciente("T-OneCotizacion-");    
    document.cookie = cookieName + "=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    actualizarCarritoBadge(0);
}

function EliminarCookie(cookieName) {
    document.cookie = cookieName + "=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
    actualizarCarritoBadge(0);
}

function getAllCookieCotizacion() {
    let cookieName = obtenerNombreCookieMasReciente("T-OneCotizacion-");
    const cookieData = getCookie(cookieName); // Obtiene la cookie.
    if (cookieData) {
        const parsedData = JSON.parse(cookieData); 
        return parsedData; // Retorna el objeto filtrado.
    }
    return null;
}

function obtenerNombreCookieMasReciente(prefijo) {    
    const cookies = document.cookie.split("; ");
    let nombreMasReciente = null;    
    cookies.forEach(cookie => {
        const [nombre, valor] = cookie.split("=");
        if (nombre.startsWith(prefijo)) {
            try {
                const datos = JSON.parse(decodeURIComponent(valor));
                if (datos.Borrador === "N") {
                    // Guarda solo el nombre de la cookie más reciente encontrada
                    nombreMasReciente = nombre;
                }
            } catch (error) {
                console.error(`Error al parsear cookie ${nombre}:`, error);
            }
        }
    });

    return nombreMasReciente;
}

function actualizarBorradorCookie() {
    let cookieName = obtenerNombreCookieMasReciente("T-OneCotizacion-");
    const cookieData = getCookie(cookieName);
    if (cookieData) {
        const parsedData = JSON.parse(cookieData);

        // Modificar el campo "Borrador"
        parsedData.Borrador = "Y";

        // Actualizar la cookie
        const expires = new Date();
        expires.setTime(expires.getTime() + 7 * 24 * 60 * 60 * 1000); // Expira en 7 días
        document.cookie = cookieName + "=" + JSON.stringify(parsedData) + "; path=/; expires=" + expires.toUTCString();
        console.log("Cookie actualizada correctamente:", parsedData);
    } else {
        console.warn("No se encontró la cookie " + cookieName);
    }
}

function ActualizaBorrador(cookieName) {
    // Obtener todas las cookies
    const cookies = document.cookie.split('; ');

    cookies.forEach(cookie => {
        const [name, value] = cookie.split('=');

        // Verificar si la cookie empieza con "T-OneCotizacion-"
        if (name.startsWith("T-OneCotizacion-")) {
            let parsedData;

            try {
                // Decodificar correctamente el valor de la cookie antes de parsearlo
                const decodedValue = decodeURIComponent(value);
                parsedData = JSON.parse(decodedValue);  // Luego parseamos como JSON
            } catch (e) {
                console.error("Error al parsear la cookie:", name, e);
                return;
            }

            // Si es la cookie actual, poner "N", de lo contrario "Y"
            parsedData.Borrador = (name === cookieName) ? "N" : "Y";

            // Actualizar la cookie
            const expires = new Date();
            expires.setTime(expires.getTime() + 7 * 24 * 60 * 60 * 1000); // Expira en 7 días

            // Solo usamos stringify sin encodeURIComponent, para evitar doble codificación
            document.cookie = name + "=" + JSON.stringify(parsedData) + "; path=/; expires=" + expires.toUTCString();
            console.log("Cookie actualizada:", name, parsedData);
        }
    });
}



