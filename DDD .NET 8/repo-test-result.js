import { check } from "k6";
import http from 'k6/http';

export const options = {
    stages: [
        { duration: "10s", target: 1 },
        { duration: "50s", target: 1 }
    ]
}

const resultUrl = 'http://localhost:65257/api/test/account/63a2901e-e67b-4a91-9399-08dd621718a9';

export default function () {
    const response = http.get(resultUrl);

    check(response, {
        'response code was 204': (res) => res.status = 204
    });
}