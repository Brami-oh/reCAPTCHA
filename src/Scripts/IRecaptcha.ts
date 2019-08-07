interface IRecaptcha {
    render(tagOrId: HTMLElement | string, props: IRenderProps): number;
    execute(keyOrId: string | number, props: any): void;
}