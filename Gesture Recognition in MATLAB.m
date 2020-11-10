%%Assignment 1 - Computer Vision, ENEL808, Min Choi 1132188

v =  VideoReader('vid4.mp4');

detector = vision.ForegroundDetector('NumGaussians', 50);%foreground detector

se2 = strel('disk',10);%setting for image processing

blobAnalysis = vision.BlobAnalysis('BoundingBoxOutputPort', true, ...
    'AreaOutputPort', false, 'CentroidOutputPort', false, ...
    'MinimumBlobArea', 1200);%blobanalysis for bounding box
finger = [];
bboxX = [];
bboxY = 0;
while hasFrame(v)
    frame  = readFrame(v);
    F = imresize(frame,0.5); % half the size of frame for faster processing

    foreground = detector(F); % detect foreground
    BW2 = imopen(foreground, se2); %do an opening to remove small noise pixels
    BW3 = imclose(BW2, se2); %closing to try connect nearby pixels
    BW5 = bwconvhull(BW3,'union'); %creates convexhull around useful area
    bbox = blobAnalysis(BW5);   %bounding box for the convexhull
    
    Rmin = 12;
    Rmax = 40;
    [centers, radii] = imfindcircles(BW3,[Rmin Rmax],'Sensitivity', 0.85,'EdgeThreshold',0.7);
    circles = length(radii); %find circular parts which are the fingertips
    
    if circles>0
        finger = [finger, circles]; % if circles are found number of circles = finger
    end
    
    if ~isempty(bbox)   % if bounding box exist
        bboxX = [bboxX, bbox(1)]; % bboxX is the x coorninate
        bboxY = [bboxY, bbox(2)]; % bboxY is the y coorninate
        [mY,nY] = size(bboxY); % number of rows and columns the array has
        maxXval = max(bboxX); % maximum value inside the aray
        minXval = min(bboxX); % minimum value inside the aray

        
        if nY>5
            vertical = bboxY(1,end-5) - bboxY(1,end);
            if vertical>0
%                 disp('UP')
            elseif vertical<0
%                 disp('DOWN')
            else
%                 disp('FLAT')
            end
        end
        triggerX = maxXval-minXval; % trying to mitigate false trigger
        if (triggerX >500)
            [maxrow, maxcol] = find(ismember(bboxX, max(bboxX(:)))); % coordinate of where the maximum exist
            [minrow, mincol] = find(ismember(bboxX, min(bboxX(:)))); % coordinate of where the minimum exist
            swipe = maxcol-mincol;
            if swipe>0
                disp('LEFT')
            elseif swipe<0
                disp('RIGHT')
            end
            swipe = 0;
            bboxX=[];
            maxcol=[];
            mincol=[];
            maxXval=[];
            minXval=[];
            triggerX=0;
        end

    end
    
    txt = sprintf('Finger count: %d', circles);
    It = insertText(F,[10 280],txt,'FontSize',30); % live display of fingers detected

    final = insertShape(It, 'Rectangle', bbox, 'Color', 'green',...
                                  'Linewidth', 3); %insert boundingbox to the frame
                          
    imshow(final)
    hold on
    viscircles(centers, radii,'Color','r');
    hold off % display the processed image frames

    pause(0.001)
end