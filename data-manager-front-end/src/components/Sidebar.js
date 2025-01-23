import {FaAngleLeft, FaAngleRight} from "react-icons/fa";
import {createContext, useContext, useState} from "react";
import {Link, useMatch, useResolvedPath} from "react-router-dom";


const SidebarContext = createContext();

export function Sidebar({children}) {
    const [expanded, setExpanded] = useState(true);
    return (<aside className="h-screen">
        <nav className="h-full flex flex-col bg-gray-100 border-r shadow-sm">
            <div className="p-4 pb-2 flex justify-between items-center">
                <img src="https://img.logoipsum.com/285.svg"
                     className={`overflow-hidden transition-all ${expanded ? "w-32" : "w-0"}`} alt="logo"/>
                <button onClick={() => setExpanded(current => !current)}
                        className="p-1.5 rounded-lg bg-white hover:bg-gray-200">{expanded ? <FaAngleLeft/> :
                    <FaAngleRight/>}</button>
            </div>

            <SidebarContext.Provider value={{expanded}}>
                <ul className="flex-1 px-3">
                    {children}
                </ul>
            </SidebarContext.Provider>

            <div className="border-t flex p-3">
                <img src="https://img.logoipsum.com/338.svg" className="w-10 h-10 rounded-md" alt="logo"/>
                <div
                    className={`flex justify-between items-center overflow-hidden transition-all ${expanded ? "w-32 ml-3" : "w-0"}`}>
                    <div className="leading-4">
                        <h4 className="font-semibold">Johnny Boi</h4>
                        <span className="text-xs text-gray-600">email@email.com</span>
                    </div>
                </div>
            </div>
        </nav>
    </aside>);
}

export function SidebarItem({icon, text, active, route, alert}) {
    const {expanded} = useContext(SidebarContext);

    const resolvedPath = useResolvedPath(route);    
    const isActive = useMatch({path: resolvedPath.pathname, end : true});
    
    return (<Link to={`/${route}`}>
        <li className={`relative flex items-center py-2 px-3 my-1 h-12 font-medium rounded-md cursor-pointer transition-colors group
                        ${isActive ? "bg-gradient-to-tr from-green-200 to-green-100 text-green-800" : "hover:bg-green-100 text-gray-600"}`}>
            {icon}
            <span className={`overflow-hidden transition-all ${expanded ? "w-52 ml-3" : "w-0"}`}>
           {text}
        </span>

            {!expanded && (<div className={`absolute left-full rounded-md px-2 py-1 ml-6
            bg-green-100 text-green-800 text-sm 
            invisible opacity-20 -translate-x-3 transition-all 
            group-hover:visible group-hover:opacity-100 group-hover:translate-x-0`}>
                {text}
            </div>)}
        </li>
    </Link>);
}