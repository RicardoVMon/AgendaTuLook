document.addEventListener('DOMContentLoaded', function () {
    const starsView = document.querySelectorAll('#star-rating-view .fa-star');
    const ratingValueView = document.getElementById('rating-value-view');
    const ratingDescriptionView = document.getElementById('rating-description-view');
    const comentarioReviewView = document.getElementById('ComentarioReviewView');

    const descriptions = {
        0: "",
        1: "Muy malo",
        2: "Malo",
        3: "Regular",
        4: "Bueno",
        5: "Excelente"
    };

    function updateStarsView(rating) {
        starsView.forEach(star => {
            const starValue = parseInt(star.getAttribute('data-rating'));
            if (starValue <= rating) {
                star.classList.add('active');
            } else {
                star.classList.remove('active');
            }
        });

        ratingValueView.textContent = `Calificación: ${rating}`;
        ratingDescriptionView.textContent = descriptions[rating] || "";
    }

    document.querySelectorAll('.btn-ver-calificacion').forEach(btn => {
        btn.addEventListener('click', function () {
            const calificacion = parseInt(this.getAttribute('data-calificacionreview'));
            const comentario = this.getAttribute('data-comentarioreview');

            updateStarsView(calificacion);
            comentarioReviewView.value = comentario || "";
        });
    });
});
