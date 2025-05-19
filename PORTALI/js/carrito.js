// Función para obtener una cookie
function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
    return null;
}

// Función para inicializar el badge al cargar la página
function inicializarCarritoBadge() {
    let cookieName = GetNombreCookie("T-OneCotizacion-");    
    const existingCotizacion = getCookie(cookieName);
    if (existingCotizacion) {        
        const cotizacion = JSON.parse(existingCotizacion);        
        actualizarCarritoBadge(cotizacion.productos.length);
    } else {
        actualizarCarritoBadge(0); // Si no hay cookie, muestra 0
    }
}

// Función para actualizar el badge del carrito
function actualizarCarritoBadge(totalProductos) {    
    const cartBadge = document.getElementById('cartBadge');
    if (cartBadge) {
        cartBadge.textContent = totalProductos;
    }
}

// Llama a esta función al cargar cada página
document.addEventListener('DOMContentLoaded', function () {    
    inicializarCarritoBadge();
});


function GetNombreCookie(prefijo) {
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