# Empowering the Content Author: Popular Tools Ribbon Module Documentation
**Category:** Sitecore Admin (XP) UI for Content Editors & Marketers



## Summary
  The Sitecore Admin XP UI provides a lot of default controls. Our **Popular Tools ribbon module** aims to create a more efficient content editing experience for all types of Content Author (CA; aka User) at all experience levels.

  The Content Author UI module allows a CA's activity to be tracked to create a personalized ribbon in the Content Editor view. Because the session never ends, button interactions in the editor are tracked using LiteDB. This tracked activity is then analyzed to automatically populate the following chunks:
* **Me**: individual CA's most frequently used ribbon buttons across all of their sessions in the instance
* **Everyone**: ribbon buttons used most frequently among all CEs across all sessions in the instance

  Both chunks in this module are dynamically updated and personalized with up to six buttons based on the criteria noted above. This feature is not available in the Experience Editor.

  For the Sitecore Hackathon 2019 purposes, a demo module is provided. Details are noted in the Usage section and attached video below.


## Prerequisites
  This module has the following dependency: Sitecore Experience Platform 9.1 Initial Release



## Installation
  1. Install a new instance of Sitecore Experience Platform 9.1 Initial Release using your preferred method.

  2. For instructional purposes, let's say the hostname is: local.sc

  3. Log into Sitecore at local.sc/sitecore.

  4. From the Launch Pad, open the Desktop application.

  5. Use the Sitecore Installation wizard (Start menu > Development Tools > Installation Wizard) to install the Popular_Tools.zip file.

  The module is now available for Content Authors to use. 



## Usage


### Active State
  
  Each chunk -- Me and Everyone -- can contain up to six buttons. The six buttons that display in each chunk are based on the criteria noted in the Summary section above. Selecting a button in any ribbon of the Editor will refresh the page, allowing each chunk's button set to be updated during a CA's active session. If less than six buttons are listed in the Me chunk, then a button can be added by selecting it at least five times. Shortcut key usage does not count. Example:
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;![Active State](https://github.com/Sitecore-Hackathon/2019-Hack-Pack/blob/master/documentation/images/Active_State-HackPack_2019SCHackathon.png "Active State HackPack 2019SCHackathon")
   
### Default State
  The default ribbon that displays in the Content Editor only includes the Everyone chunk. The Me chunk will start to populate once the CA begins to interact with any button(s) in any of its ribbons. Example:
  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;![Default State](https://github.com/Sitecore-Hackathon/2019-Hack-Pack/blob/master/documentation/images/Default_State-HackPack_2019SCHackathon.png "Default State HackPack 2019SCHackathon")
  
### Fallback State
   This alternative Popular Tools ribbon version displays for CAs in the Content Editor if both button sets are empty; for example, if the CA logging in is the very first user to ever log into the Sitecore instance. This ribbon has two default buttons:

   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;a. **Publish** | _Will display the parent dropdown version of the button, allowing the CA to configure their publishing options in the Publish modal before initiating the publish._

   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;b. **Experience Editor**
   
 Example:
   
 ![Fallback State](https://github.com/Sitecore-Hackathon/2019-Hack-Pack/blob/master/documentation/images/Fallback_State-HackPack_2019SCHackathon.png "Fallback State HackPack 2019SCHackathon")

### Notes: 
* The restrictions behavior of ribbon buttons is inherited from the out-of-the-box Sitecore settings (1). If the CA does not have the necessary permissions (2) then the button is disabled. Example:
   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;![Disabled Button States](https://github.com/Sitecore-Hackathon/2019-Hack-Pack/blob/master/documentation/images/Disabled_Buttons-HackPack_2019SCHackathon.png "Disabled Button States HackPack 2019SCHackathon")
* The Save button is filtered out of both chunks -- including the fallback state -- because it displays in all ribbons by default
* To avoid duplicate buttons between the two chunks: if a button listed in the Everyone chunk is added to a CA's Me chunk, then the button is removed from the Everyone chunk of that CA's account. The button will still display in the Everyone chunk of other CA accounts if the button is not part of their top six frequently used buttons
* The aggregation agent is configured to run every 10 seconds, so it will customize your ribbon experience during your session
    
Please watch the video for additional details: ![2019 Sitecore Hackathon - Popular Tools Customized Ribbon Module: DEMO](https://www.youtube.com/watch?v=vDX-kE7D_k4 "")

## Value

### Content Editors and Marketers

   **Novices:** looking at the Content Editor view for the first time can be overwhelming with how many default controls there are. We’ve seen this first hand with many new CAs. As these CAs start to dip their toes into the Editor waters, building their list of commonly used buttons that they come back to most will help reassure them where to find “the familiars” to fall back on as they continue to explore all of the new territory. Including the Experience Editor button in the fallback state helps to promote a shortcut to this view, which is typically seen as easier to consume by new CAs.

   **Advanced:** these CAs have used Sitecore before and can get around the Content Editor view. But as the Sitecore versions evolve, and the controls change, maintaining their advanced status can become a challenge. This tool can help these users stay in the know -- and grow!

   **Experts:** these users are so experienced with Sitecore that it can become a nuisance to have to navigate around all of the various ribbons in the Content Editor view to complete their task(s). There is a limit to how quickly they can navigate through all of those menus, ribbons, and buttons -- not any more! Now, the well known path(s) can become simplified: offering true efficiency improvement by significantly shortening the amount of time spent doing frequent tasks. 

   **Trainers:** A CA trying to train another CA can use the list in each chunk as a foundation. It may offer an overview list of must-knows and starting points to cover in training sessions. 
