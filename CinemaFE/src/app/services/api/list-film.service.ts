import { Injectable, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ListFilm } from 'src/app/model/list-film';
import { Observable } from 'rxjs';
import { BaseApiService } from '../baseapi';

@Injectable({
    providedIn: 'root',
})
export class ListFilmService {
    constructor(private http: HttpClient) {}
     baseUrl = new BaseApiService; 
    // GET

    public getlist(): Observable<any> {
        console.log(this.baseUrl.rootUrl)
        return this.http.get<any>(this.baseUrl.rootUrl + 'Movie'); // link truyen vao day
    }
}
