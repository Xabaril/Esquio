import tl = require('./node_modules/azure-pipelines-task-lib/task');
import url = require('url');
const https = require('https');

async function run() {
    try {
        const esquioConnection = tl.getInput('EsquioService', true);
        const productName: string = tl.getInput('productName', true);
        const featureName: string = tl.getInput('featureName', true);
        const deploymentName: string = tl.getInput('deploymentName', true);

        const esquioUrl = url.parse(tl.getEndpointUrl(esquioConnection, false));
        const serverEndpointAuth: tl.EndpointAuthorization = tl.getEndpointAuthorization(esquioConnection, false);
        const esquioApiKey = serverEndpointAuth["parameters"]["apitoken"];

        await rollbackFeature(esquioUrl, esquioApiKey, productName, featureName, deploymentName);
    }
    catch (err) {
        tl.setResult(tl.TaskResult.Failed, err.message);
    }
}

async function rollbackFeature(esquioUrl: url.UrlWithStringQuery, esquioApiKey: string, productName: string, featureName: string, deploymentName: string) {
    const options = {
        hostname: esquioUrl.host,
        path: `/api/products/${productName}/deployments/${deploymentName}/features/${featureName}/rollback`,
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'x-api-key': esquioApiKey,
            'x-api-version': '3.0'
        }
    }
    const req = https.request(options, (res: any) => {
        if (res.statusCode === 200) {
            console.log('Feature rollback succesful');
        }

        res.on('data', (data: any) => {
            if (res.statusCode != 200) {
                const responseData = JSON.parse(data);
                tl.setResult(tl.TaskResult.Failed, `Error in feature rollback ${responseData.detail} HttpCode: ${res.statusCode}`);
            }
        });
    });
    req.on('error', (error: any) => {
        tl.setResult(tl.TaskResult.Failed, error);
    });

    req.end();
}

run();