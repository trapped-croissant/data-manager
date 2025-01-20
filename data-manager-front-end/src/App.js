import logo from './logo.svg';
import './App.css';

function App() {
    return (
        <div className="App">
            <header className="App-header">
                <img src={logo} className="App-logo" alt="logo"/>
                <p>
                    Edit <code>src/App.js</code> and save to reload.
                </p>
                <a
                    className="App-link"
                    href="https://reactjs.org"
                    target="_blank"
                    rel="noopener noreferrer"
                >
                    Learn React!
                </a>

                <button onClick={handleClick}>Click for file</button>
            </header>
        </div>
    );
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

export default App;
