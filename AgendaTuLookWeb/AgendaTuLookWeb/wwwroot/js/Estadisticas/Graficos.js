document.addEventListener('DOMContentLoaded', function () {
    // --- Gráfico Pie de Reviews ---
    const ctx1 = document.getElementById('chartReviewsPosVsNeg');
    if (ctx1) {
        new Chart(ctx1.getContext('2d'), {
            type: 'pie',
            data: {
                labels: ['Positivas', 'Negativas'],
                datasets: [{
                    data: JSON.parse(ctx1.dataset.reviewsData),
                    backgroundColor: ['#28a745', '#dc3545']
                }]
            },
            
        });
    }

    // --- Gráfico de Ingresos por Servicio ---
    const ctx2 = document.getElementById('chartIngresosPorServicio');
    if (ctx2) {
        const servicios = JSON.parse(ctx2.dataset.servicios);
        const ingresos = JSON.parse(ctx2.dataset.ingresos);

        new Chart(ctx2.getContext('2d'), {
            type: 'bar',
            data: {
                labels: servicios,
                datasets: [{
                    label: 'Ingresos por Servicio',
                    data: ingresos,
                    backgroundColor: '#007bff',
                    borderRadius: 5
                }]
            },
        });
    }
});
