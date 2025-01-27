import {downloadFile} from "../data/file-manager";

const Api_Host_Url_V1 = 'https://localhost:7084/api/v1/generatedata';

export async function getGenerateDataFile(columns, rows) {
    return await fetch(`${Api_Host_Url_V1}?columns=${columns}&records=${rows}`)
        .then((data) => data.blob())
        .then((blob) => downloadFile(blob));
}