document.querySelectorAll('.day-selector').forEach(selector => {
    selector.addEventListener('change', function () {
        const contenedorHoras = document.getElementById('contenedor-horas');
        const horaInicioInput = document.getElementById('hora-inicio');
        const horaFinInput = document.getElementById('hora-fin');
        const botonActivarDesactivar = document.getElementById('cambiarEstado');

        // Obtener valores de los atributos data-* (hora de inicio, hora de fin, id del día)
        const startTime = this.getAttribute('data-start');
        const endTime = this.getAttribute('data-end');
        const diaId = this.getAttribute('data-id');

        // Asignar valores a los inputs de hora
        horaInicioInput.value = startTime;
        horaFinInput.value = endTime;

        // Mostrar el contenedor de horas
        contenedorHoras.classList.remove('d-none');

        // Añadir el id del día en el botón
        botonActivarDesactivar.setAttribute('data-id', diaId);
    });
});
