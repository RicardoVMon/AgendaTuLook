document.getElementById('cambiarEstado').addEventListener('click', function () {
    const diaTrabajoId = this.getAttribute('data-id');  // Obtener el ID del día desde el botón
    const boton = this;
    $.ajax({
        url: '/DiasTrabajo/CambiarEstadoDiaTrabajo',
        type: 'POST',
        data: {
            diaTrabajoId: diaTrabajoId
        },
        success: function (response) {
            if (response.success) {
                Swal.fire({
                    title: "Éxito",
                    text: response.message,
                    icon: "success"
                }).then(() => {
                    // Actualizar el color del label (de la selección del día)
                    const label = document.querySelector(`label[for='dia-${diaTrabajoId}']`);
                    if (label.classList.contains('btn-outline-success')) {
                        label.className = "btn btn-outline-secondary";
                    } else {
                        label.className = "btn btn-outline-success";
                    }
                });
            }
        }
    });
});
