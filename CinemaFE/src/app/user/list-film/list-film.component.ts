import { Component, OnInit } from '@angular/core';
import { ListFilmService } from '../../services/api/list-film.service';
import { ListFilm } from '../../model/list-film';

@Component({
    selector: 'app-list-film',
    templateUrl: './list-film.component.html',
    styleUrls: ['./list-film.component.css'],
})
export class ListFilmComponent implements OnInit {
    listFilm:any
    constructor(private film: ListFilmService) {}

    ngOnInit(): void {
        
        this.film.getlist().subscribe((res) => {
            this.listFilm = res.data;
            console.log(res )
        });
    }
}
