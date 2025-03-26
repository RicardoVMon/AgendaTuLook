// Función para actualizar las horas en el servidor
function actualizarHoras(diaId, nuevaHoraInicio, nuevaHoraFin) {
    $.ajax({
        url: '/DiasTrabajo/ActualizarHorasDiaTrabajo',
        type: 'POST',
        data: {
            diaTrabajoId: diaId,
            nuevaHoraInicio: nuevaHoraInicio,
            nuevaHoraFin: nuevaHoraFin
        },
        success: function (response) {
            if (response.success) {
                Swal.fire({
                    title: "Éxito",
                    text: response.message,
                    icon: "success"
                });
            } else {
                Swal.fire({
                    title: "Error",
                    text: "No se pudo actualizar las horas.",
                    icon: "error"
                });
            }
        },
        error: function () {
            Swal.fire({
                title: "Error",
                text: "Hubo un problema al procesar la solicitud.",
                icon: "error"
            });
        }
    });
}

// Función común para manejar cambios en los inputs de hora
function manejarCambioHora() {
    const diaId = document.getElementById('cambiarEstado').getAttribute('data-id');
    const nuevaHoraInicio = document.getElementById('hora-inicio').value;
    const nuevaHoraFin = document.getElementById('hora-fin').value;

    // Actualizar las horas en el servidor
    actualizarHoras(diaId, nuevaHoraInicio, nuevaHoraFin);
}

// Escuchar cambios en los inputs de hora
document.getElementById('hora-inicio').addEventListener('change', manejarCambioHora);
document.getElementById('hora-fin').addEventListener('change', manejarCambioHora);
