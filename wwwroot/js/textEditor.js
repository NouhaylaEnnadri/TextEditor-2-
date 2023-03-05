﻿// Load the Quill editor
var quill = new Quill("#documentEditor", {
    modules: {
        toolbar: [
            ["bold", "italic", "underline", "strike"],
            [{ header: 1 }, { header: 2 }, { header: 3 }, { header: 4 }, { header: 5 }, { header: 6 }],
            [{ list: "ordered" }, { list: "bullet" }],
            [{ script: "sub" }, { script: "super" }],
            [{ indent: "-1" }, { indent: "+1" }],
            [{ direction: "rtl" }],
            [{ size: ["small", false, "large", "huge"] }],
            [{ color: [] }, { background: [] }],
            [{ font: [] }],
            [{ align: [] }],
            [{ image: [] }, { video: [] }, { formula: [] }],
            ["link"],
            ["clean"],
        ]

    },
    theme: "snow",
});

// Load the latest saved content from the server into the Quill editor
fetch("/Text/GetTextContent")
    .then(function (response) {
        return response.text();
    })
    .then(function (data) {
        quill.root.innerHTML = data;
    });

// Initialize SignalR hub connection
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/hubs/texteditor")
    .withAutomaticReconnect()
    .build();



// Start the SignalR hub connection
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

            // Save the document content to the server after a delay
            setTimeout(function () {
                var textContent = quill.root.innerHTML;
                var data = { "Content": textContent };
                fetch("/Text/SaveTextContent", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(data),
                })
                    .then(function (response) {
                        if (response.ok) {
                            console.log("Text content saved successfully");
                        } else {
                            console.log("Failed to save text content");
                        }
                    })
                    .catch(function (error) {
                        console.log("Error saving text content: " + error);
                    });
            }, 5000); // Save the document content after 5 seconds of inactivity
        });
    })
    .catch(function (error) {
        console.error("Error connecting to SignalR hub:", error);
    });
