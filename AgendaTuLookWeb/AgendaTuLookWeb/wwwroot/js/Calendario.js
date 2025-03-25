document.addEventListener('DOMContentLoaded', function () {
    var calendarEl = document.getElementById('calendar');

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
        //events: function (fetchInfo, successCallback, failureCallback) {
        //    $.ajax({
        //        url: '/Calendario/ObtenerCitas',
        //        method: 'GET',
        //        data: {
        //            id: 0
        //        },
        //        success: function (data) {
        //            successCallback(data);
        //        },
        //        error: function () {
        //            failureCallback();
        //        }
        //    });
        //},
        //eventClick: function (info) {
        //    var eventModal = document.getElementById('detalleEventoModal');

        //    $.ajax({
        //        url: '/Calendario/ObtenerCitas',
        //        method: 'GET',
        //        data: { id: info.event.id },
        //        success: function (data) {
        //            if (data && data.length > 0) {
        //                // Llenar los campos del modal con los datos del evento
        //                updateElementText(eventModal, '#event-title', data[0].title);
        //                updateElementText(eventModal, '#event-description', data[0].description);

        //                // Usuario/Fisioterapeuta
        //                updateElementLink(eventModal, '#event-user-link',
        //                    `/Personal/Gestion?p=${data[0].userid}`, data[0].username);

        //                // Paciente
        //                updateElementLink(eventModal, '#event-patient-link',
        //                    `/Paciente/FichaClinica?p=${data[0].patientid}`, data[0].patientname);

        //                // Fechas
        //                updateElementText(eventModal, '#event-start-time',
        //                    new Date(data[0].start).toLocaleString('es-ES', { hour12: true }));
        //                updateElementText(eventModal, '#event-end-time',
        //                    new Date(data[0].end).toLocaleString('es-ES', { hour12: true }));

        //                // Botones de acción - verificar si existen antes de configurarlos
        //                var deleteButton = eventModal.querySelector('#delete-event-button');
        //                if (deleteButton) {
        //                    deleteButton.href = `/Calendario/EliminarCita?a=${data[0].id}`;
        //                }

        //                // Mostrar el modal
        //                var modalEvent = new bootstrap.Modal(eventModal);
        //                modalEvent.show();

        //                // Configurar el event listener para editar solo si existe el botón
        //                configureEditButton(data[0]);
        //            }
        //        },
        //        error: function () {
        //            alert('Error al obtener los detalles del evento.');
        //        }
        //    });
        //},
        //eventMouseEnter: function (info) {
        //    info.el.style.cursor = 'pointer';
        //},
        //dateClick: function (info) {
        //    // Verificar si el usuario tiene permiso para crear citas
        //    if (typeof userCanCreateAppointments !== 'undefined' && userCanCreateAppointments) {
        //        var addEventModal = document.getElementById('agregarCitaModal');
        //        var startDateInput = addEventModal.querySelector('#startDate');
        //        var startTimeInput = addEventModal.querySelector('#startTime');

        //        startDateInput.value = info.dateStr.split('T')[0];
        //        startTimeInput.value = info.dateStr.split('T')[1]?.substring(0, 5) || '00:00';

        //        var modal = new bootstrap.Modal(addEventModal);
        //        modal.show();
        //    }
        //    // Si no tiene permisos, simplemente no hace nada
        //}
    });
    calendar.render();

    //// Función de utilidad para actualizar el texto de un elemento si existe
    //function updateElementText(parent, selector, text) {
    //    const element = parent.querySelector(selector);
    //    if (element) {
    //        element.textContent = text || '';
    //    }
    //}

    //// Función de utilidad para actualizar links
    //function updateElementLink(parent, selector, href, text) {
    //    const element = parent.querySelector(selector);
    //    if (element) {
    //        element.href = href || '#';
    //        element.textContent = text || '';
    //    }
    //}

    //// Función para configurar el botón de editar
    //function configureEditButton(eventData) {
    //    const editButton = document.getElementById('edit-event-button');
    //    // Solo configurar el botón si existe
    //    if (editButton) {
    //        editButton.addEventListener('click', function () {
    //            var editarCitaModal = document.getElementById('editarCitaModal');

    //            // Llenar los campos del modal de edición
    //            editarCitaModal.querySelector('#id').value = eventData.id;
    //            editarCitaModal.querySelector('#titulo').value = eventData.title;
    //            editarCitaModal.querySelector('#startDate').value = eventData.start.split('T')[0];
    //            editarCitaModal.querySelector('#startTime').value = eventData.start.split('T')[1].substring(0, 5);
    //            editarCitaModal.querySelector('#endTime').value = eventData.end.split('T')[1].substring(0, 5);
    //            editarCitaModal.querySelector('#descripcion').value = eventData.description;

    //            // Verificar que existan los selectores antes de asignarles un valor
    //            const pacienteSelect = editarCitaModal.querySelector('select[name="PacienteID"]');
    //            if (pacienteSelect) {
    //                pacienteSelect.value = eventData.patientid;
    //            }

    //            const usuarioSelect = editarCitaModal.querySelector('select[name="UsuarioID"]');
    //            if (usuarioSelect) {
    //                usuarioSelect.value = eventData.userid;
    //            }

    //            // Cerrar el modal de detalles y abrir el de edición
    //            var detailModal = bootstrap.Modal.getInstance(document.getElementById('detalleEventoModal'));
    //            detailModal.hide();

    //            var editModal = new bootstrap.Modal(editarCitaModal);
    //            editModal.show();
    //        }, { once: true }); // Usar once:true para evitar que se acumulen los event listeners
    //    }
    //}

    //$('#fileManagementModal').on('hidden.bs.modal', function () {
    //    $(this).find('form').trigger('reset');
    //});
});