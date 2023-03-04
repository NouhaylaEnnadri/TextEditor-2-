var quill = new Quill("#documentEditor", {
    modules: {
        toolbar: [
            ["bold", "italic", "underline", "strike"],
            [{ header: 1 }, { header: 2 }],
            [{ list: "ordered" }, { list: "bullet" }],
            [{ script: "sub" }, { script: "super" }],
            [{ indent: "-1" }, { indent: "+1" }],
            [{ direction: "rtl" }],
            [{ size: ["small", false, "large", "huge"] }],
            [{ header: [1, 2, 3, 4, 5, 6, false] }],
            [{ color: [] }, { background: [] }],
            [{ font: [] }],
            [{ align: [] }],
            ["clean"]
        ]
    },
    theme: "snow"
});

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/texteditor")
    .build();

connection
    .start()
    .then(function () {
        // Subscribe to updates from the server
        connection.on("updateDocument", function (content) {
            // Update the document content with the received update
            console.log(content);
            quill.updateContents(JSON.parse(content));
        });
        // Send updates to the server when the document content changes
        quill.on("text-change", function (delta, oldDelta, source) {
            if (source == "user") {
                connection.invoke("updateDocument", JSON.stringify(delta));
            }
            //// Extract the content of the Quill editor
            //var content = quill.root.innerHTML;

            //// Insert the content into the output element
            //document.getElementById("output").innerHTML = content;
        });
    })
    .catch(function (error) {
        console.error("Error connecting to SignalR hub:", error);
});