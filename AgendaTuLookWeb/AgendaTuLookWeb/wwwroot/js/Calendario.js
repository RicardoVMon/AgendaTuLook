document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');
    var usuarioId = document.getElementById('idUsuario').value;

    var calendar = new FullCalendar.Calendar(calendarEl, {
        locale: 'es',
        height: 650,
        nowIndicator: true,
        initialView: 'timeGridWeek',
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
        },
        buttonText: {
            today: 'Hoy',
            month: 'Mes',
            week: 'Semana',
            day: 'Día',
            listWeek: 'Lista Semanal'
        },
        allDayText: 'Hora',

        // Aquí se define el formato 12 horas con AM/PM
        slotLabelFormat: {
            hour: 'numeric',
            minute: '2-digit',
            hour12: true
        },
        eventTimeFormat: {
            hour: 'numeric',
            minute: '2-digit',
            hour12: true
        },

        events: function (fetchInfo, successCallback, failureCallback) {
            $.ajax({
                url: '/Calendario/ConsultarCitasCalendario',
                method: 'GET',
                data: {
                    id: usuarioId
                },
                success: function (data) {
                    if (Array.isArray(data)) {
                        successCallback(data);
                    } else {
                        successCallback([]);
                    }
                },
                error: function () {
                    failureCallback();
                }
            });
        },
        eventClick: function (info) {
            window.location.href = '/Citas/DetalleCita?id=' + info.event.id;
        },
    });

    calendar.render();
});
