var selectedHourBtn = null;
function seleccionarHora(btn, hora) {
    // Actualizar el valor del input oculto
    document.getElementById('horaSeleccionada').value = hora;

    // Cambiar el estilo del botón seleccionado
    if (selectedHourBtn) {
        selectedHourBtn.classList.remove('btn-primary');
        selectedHourBtn.classList.add('btn-outline-primary');
    }
    btn.classList.remove('btn-outline-primary');
    btn.classList.add('btn-primary');
    selectedHourBtn = btn;
}