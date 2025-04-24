function redirigirConfirmarEdicion() {
    const citaId = document.getElementById('citaId').value;
    const servicioNuevoId = document.getElementById('servicioNuevoId').value;
    const servicioActualId = document.getElementById('servicioActualId').value;
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

    window.location.href = `/Citas/ConfirmarEdicionCita?id=${citaId}&sn=${servicioNuevoId}&sa=${servicioActualId}&f=${fechaSeleccionada}&h=${horaSeleccionada}`;
}