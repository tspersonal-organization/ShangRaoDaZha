//
//  NativeMedia.m
//  NativeMedia
//
//  Created by Tomaz Saraiva on 28/03/2017.
//  Copyright © 2017 Add Component. All rights reserved.
//

#import "NativeMedia.h"

@interface UIDevice (MyPrivateNameThatAppleWouldNeverUseGoesHere)
-(void) setOrientation:(UIInterfaceOrientation)orientation;
@end

@implementation NativeMedia

NSString* _mediaFilename;
NSString* _mediaCallback;
NSString* _mediaGameObject;

UIImagePickerController * _picker;

NativeMedia* _instanceMedia;

UIDeviceOrientation *deviceOrientation;


- (void)selectPictureWithFilename:(NSString*)filename
                         callback:(NSString*)callback
                       gameObject:(NSString*)gameObject {
    
    [self newPictureWithFilename:filename
                        callback:callback
                      gameObject:gameObject
                            type:UIImagePickerControllerSourceTypePhotoLibrary];
}

- (void)takePictureWithFilename:(NSString*)filename
                       callback:(NSString*)callback
                     gameObject:(NSString*)gameObject {
    
    deviceOrientation = [[UIDevice currentDevice] orientation];
    
    if (! [UIImagePickerController isSourceTypeAvailable:UIImagePickerControllerSourceTypeCamera]) {
        
        UIAlertView *deviceNotFoundAlert = [[UIAlertView alloc] initWithTitle:@"No Camera"
                                                                      message:@"Camera is not available"
                                                                     delegate:nil
                                                            cancelButtonTitle:@"Ok"
                                                            otherButtonTitles:nil];
        [deviceNotFoundAlert show];
        
    } else {
        [self newPictureWithFilename:filename
                            callback:callback
                          gameObject:gameObject
                                type:UIImagePickerControllerSourceTypeCamera];
    }
}



- (void)newPictureWithFilename:(NSString*)filename
                      callback:(NSString*)callback
                    gameObject:(NSString*)gameObject
                          type:(UIImagePickerControllerSourceType)type {
    
    _mediaFilename = filename;
    _mediaCallback = callback;
    _mediaGameObject = gameObject;
    
    _instanceMedia = self;

    _picker = [[UIImagePickerController alloc] init];
    _picker.delegate = _instanceMedia;
    _picker.allowsEditing = YES;
    _picker.sourceType = type;

    
    if( type == UIImagePickerControllerSourceTypeCamera ){
        _picker.cameraDevice = UIImagePickerControllerCameraDeviceFront;
    }

    [UnityGetGLViewController() presentViewController:_picker animated:YES completion:NULL];

}

// image picker delegate method - image selected
- (void)imagePickerController:(UIImagePickerController *)picker
didFinishPickingMediaWithInfo:(NSDictionary *)info {
    
    _picker = picker;
    UIImage *image = info[UIImagePickerControllerOriginalImage];
    
    if (image == nil) {
        NSLog(@"IMAGE NOT FOUND");
        [self imageSelected:nil];
        return;
    }
    
    // rotate image to fit correct pixel width and height
    if (image.imageOrientation == UIImageOrientationLeft)
    {
        image = [self correctImageRotation:image :image.size.width :image.size.height];
    }
    else if (image.imageOrientation == UIImageOrientationRight)
    {
        image = [self correctImageRotation:image :image.size.width :image.size.height];
    }
    else if (image.imageOrientation == UIImageOrientationUp)
    {
        // do nothing
    }
    else if (image.imageOrientation == UIImageOrientationDown)
    {
        image = [self correctImageRotation:image :image.size.width :image.size.height];
    }
    
    NSArray *paths = NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
    if (paths.count == 0) {
        NSLog(@"DIR NOT FOUND");
        [self imageSelected:nil];
        return;
    }
    
    NSString *imageName = _mediaFilename;
    if (![imageName hasSuffix:@".png"]) {
        imageName = [imageName stringByAppendingString:@".png"];
    }
    
    NSString *imageSavePath = [(NSString *)[paths objectAtIndex:0] stringByAppendingPathComponent:imageName];
    NSData *png = UIImagePNGRepresentation(image);
    if (png == nil) {
        NSLog(@"IMAGE COPY FAILED");
        [self imageSelected:nil];
        return;
    }
    
    BOOL success = [png writeToFile:imageSavePath atomically:YES];
    if (success == NO) {
        NSLog(@"IMAGE COPY FAILED");
        [self imageSelected:nil];
        return;
    }
    
    [self imageSelected:imageSavePath];
}

-(void)imageSelected:(NSString*)path {
    NSLog(@"Image Selected: %@", path);
    
    if (path!=nil) {
        UnitySendMessage([self encodeNSString:_mediaGameObject],
                           [self encodeNSString:_mediaCallback],
                           path != nil ? [self encodeNSString:path] : "");
    }

    [self dismiss];
}

// image picker delegate method - image selection canceled
- (void)imagePickerControllerDidCancel:(UIImagePickerController *)picker {
    _picker = picker;
    [self imageSelected:nil];
}

// hide the picker view controller and clear all references
-(void)dismiss {
    [_picker dismissViewControllerAnimated:NO completion:nil];
    _picker = nil;
    _instanceMedia = nil;
    _mediaFilename = nil;
    _mediaCallback = nil;
    _mediaGameObject = nil;
    
    UIDevice *currentDevice = [UIDevice currentDevice];
    [currentDevice setOrientation:deviceOrientation];
}

// utility method
- (UIImage*)correctImageRotation:(UIImage*)image :(float)width :(float)height {
    CGSize size;
    size.width = width;
    size.height = height;
    UIGraphicsBeginImageContext( size );
    [image drawInRect:CGRectMake(0, 0, width, height)];
    UIImage* newImage = UIGraphicsGetImageFromCurrentImageContext();
    UIGraphicsEndImageContext();
    
    return newImage;
}

// FROM OBJECTIVE-C TO UNITY
-(const char*)encodeNSString:(NSString*)string {
    return [string cStringUsingEncoding:NSUTF8StringEncoding];
}

+(NSString*) createNSString:(const char*)string {
    if (string)
        return [NSString stringWithUTF8String: string];
    else
        return [NSString stringWithUTF8String: ""];
}

@end

// ----------------------------------
// Unity C Link
// ----------------------------------

static NativeMedia* instanceMedia = nil;


void _SelectPicture(const char* filename, const char* callback, const char* gameObject) {
    
    if (instanceMedia == nil) {
        instanceMedia = [[NativeMedia alloc] init];
    }
    
    [instanceMedia selectPictureWithFilename:[NSString stringWithUTF8String:filename]
                                    callback:[NSString stringWithUTF8String:callback]
                                  gameObject:[NSString stringWithUTF8String:gameObject]];
}

void _TakePicture(const char* filename, const char* callback, const char* gameObject) {
    
    if (instanceMedia == nil) {
        instanceMedia = [[NativeMedia alloc] init];
    }
    
    [instanceMedia takePictureWithFilename:[NSString stringWithUTF8String:filename]
                                  callback:[NSString stringWithUTF8String:callback]
                                gameObject:[NSString stringWithUTF8String:gameObject]];
}

