import tl = require('./node_modules/azure-pipelines-task-lib/task');
import url = require('url');
const https = require('https');

async function run() {
    try {
        const esquioConnection = tl.getInput('EsquioService', true);
        const flagId: string = tl.getInput('flagId', true);
        const esquioUrl = url.parse(tl.getEndpointUrl(esquioConnection, false));

        const serverEndpointAuth: tl.EndpointAuthorization = tl.getEndpointAuthorization(esquioConnection, false);
        const esquioApiKey = serverEndpointAuth["parameters"]["apitoken"];

        await rollbackFeature(esquioUrl, esquioApiKey, flagId)
    }
    catch (err) {
        tl.setResult(tl.TaskResult.Failed, err.message);
    }
}

async function rollbackFeature(esquioUrl: url.UrlWithStringQuery, esquioApiKey: string, flagId: string) {
    const options = {
        hostname: esquioUrl.host,
        path: `/api/v1/flags/${flagId}/rollback?apikey=${esquioApiKey}`,
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        }
    }
    const req = https.request(options, (res: any) => {
        if(res.statusCode === 200){
            console.log('Feature rollback successful');
        }
    });
    req.on('error', (error: any) => {
        console.error('There has been an error in feature rollback');
    });
    
    req.end();
}

run();