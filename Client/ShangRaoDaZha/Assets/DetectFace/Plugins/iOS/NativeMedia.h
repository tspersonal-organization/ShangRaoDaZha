//
//  NativeMedia.h
//  NativeMedia
//
//  Created by Tomaz Saraiva on 28/03/2017.
//  Copyright © 2017 Add Component. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@interface NativeMedia : NSObject <UIImagePickerControllerDelegate, UINavigationControllerDelegate>

// filename: name of the picture file
// callback: name of unity method to call with the result
// gameObject: name of the gameObject that has the script with the callback
- (void)selectPictureWithFilename:(NSString*)filename callback:(NSString*)callback gameObject:(NSString*)gameObject;
- (void)takePictureWithFilename:(NSString*)filename callback:(NSString*)callback gameObject:(NSString*)gameObject;

@end

UIViewController* UnityGetGLViewController();
void UnitySendMessage(const char *, const char *, const char *);

