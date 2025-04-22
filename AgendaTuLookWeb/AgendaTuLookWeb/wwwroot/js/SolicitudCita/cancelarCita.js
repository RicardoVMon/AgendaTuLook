$(document).ready(function () {
    $(document).on('click', '.cancelar-cita', function (e) {
        e.preventDefault();
        var citaId = $(this).data('id');

        Swal.fire({
            title: '¿Estás seguro?',
            html: '¿Deseas cancelar esta cita?<br><small>Las citas canceladas con menos de 24 horas de anticipación no son reembolsables.</small>',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sí, cancelar',
            cancelButtonText: 'No, volver'
        }).then((result) => {
            if (result.isConfirmed) {
                var form = $('<form>')
                    .attr('method', 'post')
                    .attr('action', '/Citas/CancelarCita')
                    .append($('<input>')
                        .attr('type', 'hidden')
                        .attr('name', 'citaId')
                        .val(citaId))
                    .append($('input[name="__RequestVerificationToken"]').clone());

                $('body').append(form);
                form.submit();
            }
        });
    });
});
