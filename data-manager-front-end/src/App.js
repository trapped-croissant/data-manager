import './App.css';
import {FaFileImport, FaHome} from 'react-icons/fa';
import {Sidebar, SidebarItem} from "./components/Sidebar";
import {FaWandMagicSparkles} from "react-icons/fa6";
import {Route, Routes, useMatch, useResolvedPath} from "react-router-dom"
import {GenerateRandomData} from "./pages/GenerateRandomData";
import {ImportData} from "./pages/ImportData";
import {Home} from "./pages/Home";

export default function App() {
    return (
        <main className="flex">
            <Sidebar>
                <SidebarItem icon={<FaHome size={20}/>} text="Home" route=""/>
                <SidebarItem icon={<FaWandMagicSparkles size={20}/>} text="Generate Data" route="generate"/>
                <SidebarItem icon={<FaFileImport size={20}/>} text="Import File" route="import"/>
            </Sidebar>

            <Routes>
                <Route path="/" element={<Home/>}/>
                <Route path="/generate" element={<GenerateRandomData/>}/>
                <Route path="/import" element={<ImportData/>}/>
            </Routes>
        </main>
    )
        ;
}


