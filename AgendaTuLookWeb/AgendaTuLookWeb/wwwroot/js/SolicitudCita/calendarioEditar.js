document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');
    var availableDays = [];
    var selectedDateEl = null;
    var calendar = new FullCalendar.Calendar(calendarEl, {
        locale: 'es',
        height: 650,
        validRange: {
            start: new Date() // Sets today as the earliest selectable date
        },
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

        businessHours: {
            daysOfWeek: [],
            startTime: '00:00',
            endTime: '00:00'
        },

        events: function (fetchInfo, successCallback, failureCallback) {
            fetch('/Citas/ObtenerDiasLaborables')
                .then(response => response.json())
                .then(workDays => {
                    availableDays = workDays.flatMap(day => day.daysOfWeek);
                    calendar.setOption('businessHours', workDays);
                    successCallback([]);
                })
                .catch(error => {
                    console.error('Error al cargar días laborables:', error);
                    failureCallback(error);
                });
        },

        dateClick: function (info) {
            const clickedDayOfWeek = info.date.getDay();
            if (availableDays.includes(clickedDayOfWeek)) {
                const formattedDate = info.dateStr; // Cambiado a formato YYYY-MM-DD
                document.getElementById('fechaSeleccionada').value = formattedDate;

                // Limpiar selección anterior
                if (selectedDateEl) {
                    selectedDateEl.style.backgroundColor = '';
                }

                // Marcar nuevo día seleccionado
                info.dayEl.style.backgroundColor = '#e6f7ff';
                selectedDateEl = info.dayEl;
                $.ajax({
                    url: '/Citas/ConsultarHorasDisponibles',
                    method: 'POST',
                    data: {
                        fecha: formattedDate
                    },
                    success: function (response) {
                        if (response && response.length > 0) {
                            mostrarHorasDisponibles(response);
                        } else {
                            document.getElementById('selectorHoras').innerHTML =
                                '<div class="alert alert-warning text-center mt-2" role="alert">No quedan más horas disponibles para este día</div>';
                        }
                    },
                });
            } else {
                document.getElementById('selectorHoras').innerHTML =
                    '<div class="alert alert-warning text-center mt-2" role="alert">Este día no está disponible para citas</div>';
            }
        },

        dayCellDidMount: function (info) {
            const fechaCita = '@Model.Fecha.ToString("yyyy-MM-dd")';
            const cellDate = info.date.toISOString().split('T')[0];

            if (cellDate === fechaCita) {
                info.el.style.backgroundColor = '#e6f7ff';
                info.el.style.border = '2px solid #1890ff';
                selectedDateEl = info.el;
            }

            info.el.style.cursor = 'pointer';
            info.el.addEventListener('mouseenter', function () {
                if (info.el !== selectedDateEl) {
                    info.el.style.backgroundColor = '#f0f0f0';
                }
            });
            info.el.addEventListener('mouseleave', function () {
                if (info.el !== selectedDateEl && info.dateStr !== fechaCita) {
                    info.el.style.backgroundColor = '';
                }
            });
        }
    });

    function mostrarHorasDisponibles(horas) {
        const horasDisponiblesHtml = horas.map(hora =>
            `<button class="btn btn-outline-primary m-2" onclick="seleccionarHora(this,'${hora.hora}')">${hora.hora}</button>`
        ).join('');
        document.getElementById('selectorHoras').innerHTML = horasDisponiblesHtml;
    }

    calendar.render();
});
