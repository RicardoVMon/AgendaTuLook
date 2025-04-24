function redirigirConfirmar() {
    const servicioId = document.getElementById('servicioId').value;
    const fechaSeleccionada = document.getElementById('fechaSeleccionada').value;
    const horaSeleccionada = document.getElementById('horaSeleccionada').value;

    if (!fechaSeleccionada || !horaSeleccionada) {
        Swal.fire({
            icon: 'error',
            title: 'Error',
            text: 'Por favor, seleccione una fecha y una hora.'
        });
        return;
    }

    window.location.href = `/Citas/ConfirmarCita?s=${servicioId}&f=${fechaSeleccionada}&h=${horaSeleccionada}`;
}