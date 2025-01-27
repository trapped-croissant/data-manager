import {getGenerateDataFile} from "../api/generate-data";
import {useQuery} from '@tanstack/react-query';
import {useState} from "react";

export function GenerateRandomData() {
    const [columns, setColumns] = useState(null);
    const [rows, setRows] = useState(null);

    const {
        isFetching,
        isLoading,
        refetch,
        isError,
        res,
    } = useQuery({
        queryKey: ['generateDataFile'],
        queryFn: async () => {
            if (!columns || !rows) {
                //Todo : display error message
                return "";
            }
            
            await getGenerateDataFile(columns, rows);
            return "";
        },
        enabled: false
    })

    if (isLoading || isFetching) {
        return (
            <div className="h-screen w-screen flex items-center justify-center">
                Loading...
            </div>
        );
    }

    return (
        <div className="flex flex-col items-center w-full h-screen justify-center">
            <h1 className="p-4 text-2xl">Generate Random Data</h1>

            <div className="flex flex-col bg-primary p-3 rounded-md justify-center">
                <div className="flex flex-col justify-left my-3">
                    <span className={`my-1`}>Columns</span>
                    <input type='number' className={`rounded-sm p-2`} value={columns}
                           onChange={(e) => setColumns(e.target.value)}/>

                    <span className={`my-1`}>Rows</span>
                    <input type='number' mask="999,999" className={`rounded-sm p-2`} value={rows}
                           onChange={(e) => setRows(e.target.value)}/>
                </div>

                <button className="mt-3 mb-1 p-2 rounded-md bg-green-300 hover:bg-green-400 font-medium text-gray-600"
                        onClick={refetch}>
                    Generate Random
                </button>
            </div>
            <div>
                {isError.message}
            </div>
        </div>
    );
}