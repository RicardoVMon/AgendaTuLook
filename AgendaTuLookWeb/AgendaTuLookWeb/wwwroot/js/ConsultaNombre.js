function consultarNombreUsuario() {

    let cedula = document.getElementById('Identificacion').value;

    if (cedula.length == 9) {

        $.ajax({
            type: 'GET',
            url: 'https://apis.gometa.org/cedulas/' + cedula,
            dataType: 'json',
            success: function (response) {
                if (response.resultcount > 0) {
                    document.getElementById('Nombre').value = response.results[0].fullname;
                } else {
                    document.getElementById('Nombre').value = '';
                }
            }
        });
    }


}