document.addEventListener('DOMContentLoaded', function () {
    const starRating = document.getElementById('star-rating');
    const stars = starRating.querySelectorAll('.fa-star');
    const ratingValue = document.getElementById('rating-value');
    const ratingDescription = document.getElementById('rating-description');
    const calificacionInput = document.getElementById('CalificacionReview');
    let currentRating = 4; // Valor predeterminado: 4 estrellas

    // Inicializar los tooltips de Bootstrap
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
    });

    // Descripciones para cada nivel de calificación
    const descriptions = {
        0: "",
        1: "Muy malo",
        2: "Malo",
        3: "Regular",
        4: "Bueno",
        5: "Excelente"
    };

    // Función para actualizar las estrellas visualmente
    function updateStars(rating) {
        stars.forEach(star => {
            const starValue = parseInt(star.getAttribute('data-rating'));
            if (starValue <= rating) {
                star.classList.add('active');
            } else {
                star.classList.remove('active');
            }
        });
        ratingValue.textContent = `Calificación: ${rating}`;
        ratingDescription.textContent = descriptions[rating] || "";

        // Actualizar el valor del input oculto (como si fuera un select)
        calificacionInput.value = rating;
    }

    // Inicializar con 4 estrellas
    updateStars(currentRating);

    // Evento para hacer clic en una estrella
    stars.forEach(star => {
        star.addEventListener('click', function () {
            const rating = parseInt(this.getAttribute('data-rating'));
            currentRating = rating;
            updateStars(rating);
        });
    });

    // Capturar el evento para cuando se abra el modal de calificación
    document.querySelectorAll('.btn-calificar').forEach(btn => {
        btn.addEventListener('click', function () {
            // Obtener el ID de la cita y establecerlo en el campo oculto
            const citaId = this.getAttribute('data-citaid');
            document.getElementById('citaIdHidden').value = citaId;

            // Resetear a 4 estrellas por defecto cada vez que se abre el modal
            currentRating = 4;
            updateStars(currentRating);
        });
    });
});