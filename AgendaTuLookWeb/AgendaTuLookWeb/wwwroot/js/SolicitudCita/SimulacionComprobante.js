document.addEventListener("DOMContentLoaded", function () {
    const checkbox = document.getElementById("confirmDeposito");
    const fileInput = document.getElementById("Archivos");

    checkbox.addEventListener("change", function () {
        if (this.checked) {
            Swal.fire({
                title: 'Validando comprobante...',
                html: 'Espere unos segundos mientras validamos su comprobante.',
                allowOutsideClick: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });

            setTimeout(() => {
                const archivos = fileInput.files;
                if (archivos.length > 0) {
                    Swal.fire({
                        icon: 'success',
                        title: '¡Comprobante validado!',
                        text: 'Tu comprobante ha sido validado correctamente.',
                        confirmButtonColor: '#321914'
                    });
                } else {
                    Swal.fire({
                        icon: 'error',
                        title: 'Comprobante no encontrado',
                        text: 'Por favor, adjunta una imagen o PDF del comprobante antes de confirmar.',
                        confirmButtonColor: '#d33'
                    });
                    checkbox.checked = false;
                }
            }, 2000); // Simula 2 segundos de validación
        }
    });
});