import { Component, OnInit } from '@angular/core';
import { UserService } from '../../_services/user.service';

import { User } from '../../models/interfaces/user';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from './../../_services/alertify.service';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {

  user: User;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  defaultPhotoUrl = '../../../assets/default-user.png';

  constructor(private userService: UserService,
    private alertifyService: AlertifyService,
    private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.data.subscribe(
      data => {
        this.user = data['user']
      }
    );

    this.configureGalleryOptions();


  }

  configureGalleryOptions() {
    this.galleryOptions = [
      {
        width: '500px',
        height: '500px',
        imagePercent: 100,
        thumbnailsColumns: 4,
        imageAnimation: NgxGalleryAnimation.Slide,
        preview: false
      }
    ];


    this.galleryImages = this.getImages();
  }

  getImages() {
    let imageUrls = [];
    for (const imgUrl of this.user.photos) {
      imageUrls.push({
        small: imgUrl.url,
        medium: imgUrl.url,
        big: imgUrl.url,
        description: imgUrl.description
      });
    }
    return imageUrls;
  }

}
