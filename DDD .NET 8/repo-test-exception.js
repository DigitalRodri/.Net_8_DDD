import http from 'k6/http';

export const options = {
    stages: [
        {duration: "60s", target: 50}
    ]
}

const exceptionUrl = 'http://localhost:65257/api/test/account/exception/63a2901e-e67b-4a91-9399-08dd621718a9';

export default function () {
    http.get(exceptionUrl);
}