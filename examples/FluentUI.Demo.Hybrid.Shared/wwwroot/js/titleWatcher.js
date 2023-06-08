export function getTitleAndStartWatching(dotNetHelper) {
    const target = document.querySelector('title');
    const observer = new MutationObserver(function (mutations) {
        const title = mutations[0].target.nodeValue;
        console.log(`Title changed: ${title}`);
        dotNetHelper.invokeMethodAsync('OnTitleChanged', title);
    });

    const config = { subtree: true, characterData: true, childList: true };
    observer.observe(target, config);

    return target.innerHTML;
}