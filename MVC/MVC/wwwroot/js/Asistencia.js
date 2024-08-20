Highcharts.chart('asistencia', {

    title: {
        text: 'Asistencia del usuario por mes',
        align: 'left'
    },

    subtitle: {
        text: 'Porcentaje de asistencia del usuario por mes',
        align: 'left'
    },

    yAxis: {
        title: {
            text: 'Porcentaje de Asistencia (%)'
        }
    },

    xAxis: {
        categories: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
        accessibility: {
            description: 'Meses del año'
        }
    },

    legend: {
        layout: 'vertical',
        align: 'right',
        verticalAlign: 'middle'
    },

    plotOptions: {
        series: {
            label: {
                connectorAllowed: false
            },
            dataSorting: {
                enabled: true,
                sortKey: 'y'
            }
        }
    },

    series: [{
        name: 'Asistencia',
        data: [
            70, 75, 60, 80, 85, 50, 90, 95, 90, 65, 55, 45
        ]
    }],

    responsive: {
        rules: [{
            condition: {
                maxWidth: 500
            },
            chartOptions: {
                legend: {
                    layout: 'horizontal',
                    align: 'center',
                    verticalAlign: 'bottom'
                }
            }
        }]
    },

    lang: {
        contextButtonTitle: "Menú de exportación del gráfico",
        decimalPoint: ',',
        downloadJPEG: "Descargar imagen JPEG",
        downloadPDF: "Descargar documento PDF",
        downloadPNG: "Descargar imagen PNG",
        downloadSVG: "Descargar imagen SVG",
        drillUpText: "Volver a {series.name}",
        loading: "Cargando...",
        months: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
        noData: "No hay datos que mostrar",
        numericSymbols: null,
        printChart: "Imprimir gráfico",
        resetZoom: "Restablecer zoom",
        resetZoomTitle: "Restablecer zoom nivel 1:1",
        shortMonths: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
        thousandsSep: ".",
        weekdays: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
        viewFullscreen: "Ver en pantalla completa",
        printChart: "Imprimir gráfico",
        downloadCSV: "Descargar CSV",
        downloadXLS: "Descargar XLS",
        viewData: "Ver tabla de datos"
    }

});
