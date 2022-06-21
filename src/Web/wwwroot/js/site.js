document.addEventListener("DOMContentLoaded", () => {

    var config =
    {
        size: 220,
        label: "",
        min: 0,
        max: 100,
        minorTicks: 5
    }

    var gauge = new Gauge("gaugeContainer", config);

    gauge.render();

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/hub")
        .configureLogging(signalR.LogLevel.Information)
        .build();

    connection.on("deviceTelemetry", (message) => {
        console.log(new Date().toLocaleTimeString() + " device telemetry received");
        var telemetry = JSON.parse(message);
        if (telemetry) {
            gauge.redraw(telemetry.Value);
        } else {
            console.log("invalid device telemetry");
        }
    });

    async function start() {
        try {
            console.log("Connecting to SignalR...");
            await connection.start();
            console.log("SignalR is connected");
        } catch (err) {
            console.log(err);
            setTimeout(start, 3000);
        }
    };

    connection.onclose(async () => {
        await start();
    });

    start();
});

