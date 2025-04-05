$(document).ready(function () {
    // Búsqueda por nombre
    $('#searchInput').on('keyup', function () {
        var value = $(this).val().toLowerCase();
        $('.service-card').filter(function () {
            $(this).toggle($(this).attr('data-nombre').toLowerCase().includes(value));
        });
    });

    // Ordenar por nombre y precio
    $('#filterSelect').on('change', function () {
        var order = $(this).val();
        var $cards = $('.service-card');

        $cards.sort(function (a, b) {
            var aNombre = $(a).attr('data-nombre').toLowerCase();
            var bNombre = $(b).attr('data-nombre').toLowerCase();
            var aPrecio = parseFloat($(a).attr('data-precio'));
            var bPrecio = parseFloat($(b).attr('data-precio'));

            if (order === "az") {
                return aNombre.localeCompare(bNombre, 'es', { sensitivity: 'base' });
            } else if (order === "za") {
                return bNombre.localeCompare(aNombre, 'es', { sensitivity: 'base' });
            } else if (order === "precioAsc") {
                return aPrecio - bPrecio;
            } else if (order === "precioDesc") {
                return bPrecio - aPrecio;
            }
        });

        $('#serviceList').html($cards);
    });
});