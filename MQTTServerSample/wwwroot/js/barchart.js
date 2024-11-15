
    var chr = document.getElementById("barChart");
    var ctx = chr.getContext("2d");
    ctx.canvas.width = 800;
    var data = {
        type: "bar",
    data: {
        labels: ["One", "Two"],
    datasets: [{
        label: "my first dataset",
    backgroundColor: ["#F7464A", "#46BFBD", "#FDB45C"],
    fillColor: "blue",
    strokeColor: "green",
    data: [65, 53]
                }]
            }
        };


    var myfirstChart = new Chart(chr, data);
