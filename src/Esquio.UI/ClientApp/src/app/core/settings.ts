interface ISettings {
    Audience: string;
    ClientId: string;
    Authority: string;
    ApiUrl: string;
}

let settings: ISettings = null;

async function getSettings() {
    const response = await fetch('/api/config');

    if (!response.ok) {
        throw new Error('Cannot fetch settings');
    }

    settings = await response.json();
}

export {
    getSettings,
    settings
};
