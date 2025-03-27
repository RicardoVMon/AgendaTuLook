document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');
    var availableDays = []; // Almacenará los días disponibles
    var calendar = new FullCalendar.Calendar(calendarEl, {
        locale: 'es',
        height: 650,
        nowIndicator: true,
        initialView: 'dayGridMonth',
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: ''
        },
        buttonText: {
            today: 'Hoy'
        },
        allDayText: 'Hora',

        // Configuración de días laborables
        businessHours: {
            daysOfWeek: [],
            startTime: '00:00',
            endTime: '00:00'
        },

        // Eventos para cargar días laborables
        events: function (fetchInfo, successCallback, failureCallback) {
            fetch('/Citas/ObtenerDiasLaborables')
                .then(response => response.json())
                .then(workDays => {
                    // Guardar los días disponibles para usar en dateClick
                    availableDays = workDays.flatMap(day => day.daysOfWeek);

                    // Configurar días laborables
                    calendar.setOption('businessHours', workDays);

                    successCallback([]);
                })
                .catch(error => {
                    console.error('Error al cargar días laborables:', error);
                    failureCallback(error);
                });
        },

        // Manejador de clic en fecha
        dateClick: function (info) {
            // Obtener el día de la semana de la fecha clickeada (0-6)
            const clickedDayOfWeek = info.date.getDay();

            // Verificar si el día está en los días disponibles
            if (availableDays.includes(clickedDayOfWeek)) {

                const formattedDate = info.date.toISOString();
                console.log(formattedDate)

                // Llamada AJAX para obtener horas disponibles
                $.ajax({
                    url: '/Citas/ConsultarHorasDisponibles',
                    method: 'POST',
                    data: {
                        fecha: formattedDate
                    },
                    success: function (response) {
                        mostrarHorasDisponibles(response);
                    },
                    error: function (xhr, status, error) {
                        console.error('Error al obtener horas disponibles:', error);
                        alert('No se pudieron cargar las horas disponibles');
                    }
                });
            } else {
                // Día no disponible
                document.getElementById('hour-selector').innerHTML = 
                    '<div class="alert alert-warning text-center mt-2" role="alert">Este día no está disponible para citas</div>';
            }
        }
    });

    function mostrarHorasDisponibles(horas) {
        // Generar botones para las horas disponibles
        const horasDisponiblesHtml = horas.map(hora =>
            `<button class="btn btn-outline-primary m-2">${hora.hora}</button>`
        ).join('');

        // Actualizar el contenedor #hour-selector con los botones generados
        document.getElementById('hour-selector').innerHTML = horasDisponiblesHtml;
    }
    calendar.render();
});


