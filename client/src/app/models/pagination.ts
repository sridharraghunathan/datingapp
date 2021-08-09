export interface Pagination {
    currentPage: number;
    itemsPerPage: number;
    totalItems: number;
    totalPages: number;
}

//Here T is nothing but Member[] type we have use the Generic so that class can be resued
export class PaginatedResult<T> {
    result: T;
    pagination: Pagination;
}