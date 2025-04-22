function redirigirConfirmarEdicion() {
    const citaId = document.getElementById('citaId').value;
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

    // Convertir la fecha seleccionada a formato legible
    const fecha = new Date(fechaSeleccionada);
    const fechaFormateada = `${fecha.getFullYear()}-${String(fecha.getMonth() + 1).padStart(2, '0')}-${String(fecha.getDate()).padStart(2, '0')}`;

    window.location.href = `/Citas/ConfirmarEdicionCita?id=${citaId}&f=${fechaFormateada}&h=${horaSeleccionada}`;
}