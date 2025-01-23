export function GenerateRandomData() {
    return (
        <div className="flex flex-col items-center w-full h-screen justify-center">
            <h1 className="p-4 text-2xl">Generate Random Data</h1>

            <button className="p-2 rounded-md bg-green-300 hover:bg-green-400 font-medium text-gray-600" onClick={handleClick}>
                Generate Random
            </button>
        </div>
    )
}

const handleClick = async () => {
    await fetch('https://localhost:7084/api/v1/generatedata?columns=5&records=5000')
        .then((response) => response.blob())
        .then((blob) => {
            const url = window.URL.createObjectURL(blob);

            const link = document.createElement('a');
            link.href = url;
            link.setAttribute(
                'download',
                `download.csv`,
            );

            // Append to html link element page
            document.body.appendChild(link);

            // Start download
            link.click();

            // Clean up and remove the link
            link.parentNode.removeChild(link);
        })
        .catch(error => console.error(error));
}