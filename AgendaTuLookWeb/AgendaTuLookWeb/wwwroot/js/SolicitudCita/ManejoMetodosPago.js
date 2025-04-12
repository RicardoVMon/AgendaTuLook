document.addEventListener('DOMContentLoaded', function () {
    // Obtener referencias a las pestañas
    const tabButtons = document.querySelectorAll('[data-bs-toggle="tab"]');

    // Definir los campos requeridos para cada método de pago
    const camposTarjeta = [
        document.getElementById('numeroTarjeta'),
        document.getElementById('expiracionTarjeta'),
        document.getElementById('cvvTarjeta'),
        document.querySelector('[name="MetodoPago.NombreTitular"]'),
        document.querySelector('[name="MetodoPago.Direccion"]'),
        document.querySelector('[name="MetodoPago.CodigoPostal"]'),
        document.querySelector('[name="MetodoPago.Pais"]')
    ];

    const camposSinpe = [
        document.getElementById('Archivos'),
        document.getElementById('confirmDeposito')
    ];

    function actualizarCamposRequeridos(metodoPagoId) {
        // Actualizar el valor del input oculto existente
        const metodoPagoInput = document.querySelector('[name="MetodoPago.MetodoPagoId"]');
        const metodoPagoNombre = document.querySelector('[name="MetodoPago.Nombre"]');

        if (metodoPagoInput) {
            metodoPagoInput.value = metodoPagoId;
        }

        // Actualizar el nombre del método de pago
        if (metodoPagoNombre) {
            if (metodoPagoId === '1') {
                metodoPagoNombre.value = 'Tarjeta Débito/Crédito';
            } else if (metodoPagoId === '2') {
                metodoPagoNombre.value = 'SINPE Móvil';
            }
        }

        // Función para limpiar campos
        function limpiarCampos(campos) {
            campos.forEach(campo => {
                if (!campo) return;

                if (campo.type === 'checkbox' || campo.type === 'radio') {
                    campo.checked = false;
                } else if (campo.tagName === 'SELECT') {
                    campo.selectedIndex = 0;
                } else {
                    campo.value = '';
                }
            });
        }

        if (metodoPagoId === '1') { // Tarjeta
            // Activar campos requeridos para tarjeta
            camposTarjeta.forEach(campo => {
                if (campo) campo.setAttribute('required', '');
            });

            // Desactivar y limpiar campos SINPE
            camposSinpe.forEach(campo => {
                if (campo) campo.removeAttribute('required');
            });
            limpiarCampos(camposSinpe);

        } else if (metodoPagoId === '2') { // SINPE
            // Activar campos requeridos para SINPE
            camposSinpe.forEach(campo => {
                if (campo) campo.setAttribute('required', '');
            });

            // Desactivar y limpiar campos tarjeta
            camposTarjeta.forEach(campo => {
                if (campo) campo.removeAttribute('required');
            });
            limpiarCampos(camposTarjeta);
        }
    }

    // Detectar el método activo inicialmente
    const tabActivo = document.querySelector('.nav-link.active');
    if (tabActivo) {
        const metodoPagoId = tabActivo.id.split('-')[0];
        actualizarCamposRequeridos(metodoPagoId);
    }

    // Agregar listeners para cambios de pestaña
    tabButtons.forEach(tab => {
        tab.addEventListener('shown.bs.tab', function (event) {
            const metodoPagoId = event.target.id.split('-')[0];
            actualizarCamposRequeridos(metodoPagoId);
        });
    });
});