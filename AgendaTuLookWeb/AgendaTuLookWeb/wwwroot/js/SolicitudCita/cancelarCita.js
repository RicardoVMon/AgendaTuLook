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
               $.ajax({  
                   url: '/Citas/CancelarCita',  
                   type: 'POST',  
                   data: {  
                       citaId: citaId,  
                   },  
                   success: function (response) {  
                       Swal.fire(  
                           'Cancelada',  
                           'La cita ha sido cancelada exitosamente.',  
                           'success'  
                       ).then(() => {  
                           location.reload();  
                       });  
                   },  
                   error: function () {  
                       Swal.fire(  
                           'Error',  
                           'Ocurrió un error al intentar cancelar la cita. Por favor, inténtalo de nuevo.',  
                           'error'  
                       );  
                   }  
               });  
           }  
       });  
   });  
});
