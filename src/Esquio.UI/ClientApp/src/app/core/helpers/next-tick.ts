/**
 * Awaits to the new cycle of the ui
 */
export function nextTick() {
    return new Promise(resolve => {
        setTimeout(resolve, 10);
    });
}