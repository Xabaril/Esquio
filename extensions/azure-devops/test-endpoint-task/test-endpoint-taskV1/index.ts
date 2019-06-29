import tl = require('./node_modules/azure-pipelines-task-lib/task');

async function run() {
    try {
        
        console.log('Hello Esquio');
    }
    catch (err) {
        tl.setResult(tl.TaskResult.Failed, err.message);
    }
}

run();