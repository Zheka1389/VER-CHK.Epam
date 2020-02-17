export class Article {
    constructor(
        public id: string,
        public title: string,
        public category: string,
        public createdUser: string,
        public createdDate: string,
        public comment: string,
        public content: string
    ) {}
}
