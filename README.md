# ProSeeker-
My defense project for ASP.NET Core MVC course at SoftUni (October 2020 @SoftUni)
## Brief description of the functionalities
<details>
   <summary>
       <strong> Click </strong> for more detailed information
   </summary>
ProSeeker‘s main idea is to be a platform based on supply and demand. A place where professionals in a certain field can be found by regular users who need their services. (mostly professionals who work on a recommendation basis).
Users themselves can directly seek a specialist or upload an ad and receive an offer for a service. </br>

3 roles: regular user, specialist, administrator </br>

<strong>User:</strong>
- Can create/edit/delete Ad. 
- Can send inquiry to a professional and receive an offer.
- Can receive an offer from professional in two ways (from existing Ad or from sent inquiry).
- Has section with his ads only, where he can access each of them.

<strong>Specialist:</strong>
- Receive inquiries from regular users.
- Can make offers to clients (two ways : Ad/Inquiry).

<strong>Admin:</strong>
- Create/edit/delete job sub-categories;
- Create/edit/delete base job categories;
- Can create/edit/delete new surveys with questions and answers. When a certain user takes a survey, he becomes VIP for 1 week. 
VIP regular user – his ads will appear above all others, even after sorting criteria has been selected. If the user is specialist, his profile will appear above all others, even after sorting criteria has been selected. Each survey can be taken only once. 
Common actions for users and specialists:
- Both users and specialists are allowed to like specialists’ profiles, leave comments/opinions on users’ Ads and specialists’ profiles (recursively). 
- After accepting an offer, both parties receive emails with other person’s contacts.
- Update their profile info, add/change/delete avatar image.

<strong>Restrictions:</strong>
- Specialists can make only 1 offer to a certain Ad. If they try to send second offer to the same Ad, new modal window pops up and they can either cancel the attempt to make an offer or retrieve/delete the old offer and make a new one.
- Specialists can make more than 1 offers to regular user only when the user has sent an inquiry to the specialist (through the specialist profile).
- Guest users (not logged-in) are restricted to a very few actions.
- Only regular users can send an inquiry to specialists.
- Only specialists can send offers to regular users.
- Private chat is allowed for user-user / specialist-specialist. Users cannot start a private chat with specialists. This is against the main idea of the platform. 
- Server side + client side validations for all inputs.
</details>

## :hammer: Built With:
<details>
   <summary>
       <strong> Click </strong> for more detailed information
   </summary>

* <strong>.NET 5.0 <strong>
* <strong>Entity Framework Core 5.0 <strong> 
* <strong>FontAwesome<strong> (font icons)
* <strong>AutoMapper<strong> (object-to-object mapping library)
* <strong>Repository<strong> Pattern (Mainly for easier tests nad maintaining soft deletion)
* <strong>Cloudinary<strong> (file storage)
* <strong>TinyMCE<strong> (text redactor)
* <strong>HtmlSanitizer<strong> (XSS protection)
* <strong>Bootsrap 4<strong>
* <strong>JavaScript<strong> (well…)
* <strong>CSS<strong>
* <strong>HTML 5<strong>
* <strong>Moment.Js<strong> (JavaScript library for easier work with date-time)
* <strong>JQuery<strong>
* <strong>SignalR<strong> (used for real-time chat)
* <strong>WebAPI <strong>
* <strong>SendGrid<strong> (for sending emails) 
* <strong>xUnit<strong> (for testing) 

</details>

## DB Diagram
![](https://res.cloudinary.com/zmax/image/upload/v1609124986/81eec76a-fb6c-4ccf-9941-b4fe8bec34f9profilePicture.png)


## Unit tests (services test coverage)
![](https://res.cloudinary.com/zmax/image/upload/v1610213611/adfe08c8-bf3f-4958-bbc5-ea3f10ec67fcUnitTestsCoverage.png.png)
<!-- https://res.cloudinary.com/zmax/image/upload/v1610213611/adfe08c8-bf3f-4958-bbc5-ea3f10ec67fcUnitTestsCoverage.png.png -->

## :framed_picture: Some screen shots

Home
![](https://res.cloudinary.com/zmax/image/upload/v1612348857/5cb7964f-0f48-4e68-8b93-4cf14eab9c75profilePicture1.png)
Register
![](https://res.cloudinary.com/zmax/image/upload/v1612348927/5cb7964f-0f48-4e68-8b93-4cf14eab9c75profilePicture2.png)
AD
![](https://res.cloudinary.com/zmax/image/upload/v1612350104/5cb7964f-0f48-4e68-8b93-4cf14eab9c75profilePicture11.png)
Chat
![](https://res.cloudinary.com/zmax/image/upload/v1612349009/5cb7964f-0f48-4e68-8b93-4cf14eab9c75profilePicture3.png)
Admin (Job categories)
![](https://res.cloudinary.com/zmax/image/upload/v1612349065/5cb7964f-0f48-4e68-8b93-4cf14eab9c75profilePicture4.png)
User's offers
![](https://res.cloudinary.com/zmax/image/upload/v1612349129/5cb7964f-0f48-4e68-8b93-4cf14eab9c75profilePicture5.png)
New offer
![](https://res.cloudinary.com/zmax/image/upload/v1612349191/5cb7964f-0f48-4e68-8b93-4cf14eab9c75profilePicture6.png)
Specialist's profile
![](https://res.cloudinary.com/zmax/image/upload/v1612349672/5cb7964f-0f48-4e68-8b93-4cf14eab9c75profilePicture7.png)
Specialists
![](https://res.cloudinary.com/zmax/image/upload/v1612349747/5cb7964f-0f48-4e68-8b93-4cf14eab9c75profilePicture8.png)
Survey
![](https://res.cloudinary.com/zmax/image/upload/v1612349802/5cb7964f-0f48-4e68-8b93-4cf14eab9c75profilePicture9.png)
Admin (surveys)
![](https://res.cloudinary.com/zmax/image/upload/v1612349861/5cb7964f-0f48-4e68-8b93-4cf14eab9c75profilePicture10.png)
